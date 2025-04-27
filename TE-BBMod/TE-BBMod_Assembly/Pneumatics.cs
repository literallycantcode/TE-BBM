using System;
using System.Linq;
using Modding;
using Modding.Blocks;
using static Modding.ModKeys;
using Console = Modding.ModConsole;
using UnityEngine;
using System.Collections.Generic;

namespace MysticFix
{

   class Pneumatics : FrameDelayAction
    {
        protected override int FRAMECOUNT => 1;
        protected override bool DESTROY_AT_END => false;
        private ConfigurableJoint CJ;
        private Rigidbody rigg;
        public SuspensionController SC;
        public Block thisblock;

        //Mapper objects
        private MKey ExtendKey;
        private MKey RetractKey;
        private MMenu MoveMode;
        private MSlider SpringSlider;
        private MSlider FeedSlider;
        private MSlider ExtendLimitSlider;
        private MSlider RetractLimitSlider;
        private MSlider DampSlider;

        //Mapper value variables
        private float Feed = 0.5f;
        private float ExtendLimit = 1f;
        private float RetractLimit = 1f;
        private float Dampening = 1.2f;
        private int selectedmovemode = 0;
        
        private bool Break = false;
        private bool HasBroken = false;
        private bool EKpressed = false;
        private bool RKpressed = false;
        private bool isFirstFrame = true;
        private AudioSource Breaksound;

        internal static List<string> MoveModes = new List<string>()
        {
            "None",
            "Hydraulic",
            "Pneumatic",
        };

        internal static MessageType LS;

        internal static void SetupNetworking()
        {
            LS = ModNetworking.CreateMessageType(DataType.Block);
            ModNetworking.Callbacks[LS] += StopLoopSoundClient;
        }

        protected override void DelayedAction()
        {
            //Physics stuff
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                CJ = GetComponent<ConfigurableJoint>();
                if (CJ)
                {
                    switch (selectedmovemode)
                    {
                        case 0: //None 
                            CJ.breakForce = 35000;
                            CJ.breakTorque = 30000;
                            break;

                        case 1: //Hydraulic
                            CJ.breakForce = 60000;
                            CJ.breakTorque = 60000;
                            break;

                        case 2: //Pneumatic
                            CJ.breakForce = 80000;
                            CJ.breakTorque = 80000;
                            break;
                    }
                    float limit = Mathf.Max(ExtendLimit, RetractLimit);
                    SoftJointLimit SJlimit = CJ.linearLimit;
                    SJlimit.limit = limit;
                    CJ.linearLimit = SJlimit;
                }
                rigg = GetComponent<Rigidbody>();
            }
        }
        private void Awake()
        {
            base.Awake();
            Console.Log("Pneumatics Added to "+gameObject.name);
            SC = GetComponent<SuspensionController>();

            //Mapper definition
            MoveMode = SC.AddMenu("MoveMode", selectedmovemode, MoveModes, false);
            MoveMode.ValueChanged += (ValueHandler)(value => { selectedmovemode = value; UpdateMapper(); });

            ExtendKey = SC.AddKey("extend", "Extend", KeyCode.M);

            RetractKey = SC.AddKey("retract", "Shrink", KeyCode.N);

            SpringSlider = SC.SpringSlider;
           
            FeedSlider = SC.AddSlider("feedSpeed", "feed", Feed, 0f, 25f);
            FeedSlider.ValueChanged += (float value) => { Feed = value; };

            ExtendLimitSlider = SC.AddSlider("extendLimit", "ExtendLimit", ExtendLimit, 0f, 4f);
            ExtendLimitSlider.ValueChanged += (float value) => { ExtendLimit = value; };

            RetractLimitSlider = SC.AddSlider("retractLimit", "ShrinkLimit", RetractLimit, 0f, 4f);
            RetractLimitSlider.ValueChanged += (float value) => { RetractLimit = value; };

            DampSlider = SC.AddSlider("Dampening", "Dampening", Dampening, 1.2f, 10f);
            DampSlider.ValueChanged += (float value) => { Dampening = value * 10000; };

            MoveMode.DisplayInMapper = true;
            Breaksound = SC.gameObject.AddComponent<AudioSource>();
            Breaksound.spatialBlend = 1;
            Breaksound.maxDistance = 150f;
            Breaksound.rolloffMode = AudioRolloffMode.Linear;
            Breaksound.volume = 0.3f;
            Breaksound.playOnAwake = false;
            Breaksound.clip = ModResource.GetAudioClip("air_Broken");
            Breaksound.reverbZoneMix = 0.05f;
            
            thisblock = Block.From(SC);
        }

        private void UpdateMapper()
        {
            switch (selectedmovemode)
            {
                case 0: //None
                    ExtendKey.DisplayInMapper = false;
                    RetractKey.DisplayInMapper = false;
                    FeedSlider.DisplayInMapper = false;
                    ExtendLimitSlider.DisplayInMapper = false;
                    RetractLimitSlider.DisplayInMapper = false;
                    DampSlider.DisplayInMapper = false;
                    //SC.SpringSlider can be null on stripped blocks in multiverse.
                    if (SpringSlider != null) SpringSlider.SetRange(0, 3f);
                    if (SpringSlider != null && SpringSlider.Value > SpringSlider.Max) SpringSlider.SetValue(SpringSlider.Max);
                    break;

                case 1: //HydraulicMode
                    ExtendKey.DisplayInMapper = true;
                    RetractKey.DisplayInMapper = true;
                    FeedSlider.DisplayInMapper = true;
                    ExtendLimitSlider.DisplayInMapper = true;
                    RetractLimitSlider.DisplayInMapper = true;
                    DampSlider.DisplayInMapper = false;
                    FeedSlider.SetRange(0f, 25f);
                    //SC.SpringSlider can be null on stripped blocks in multiverse.
                    if (SpringSlider != null) SpringSlider.SetRange(0, 200f);
                    break;

                case 2: //PneumaticMode
                    ExtendKey.DisplayInMapper = true;
                    RetractKey.DisplayInMapper = true;
                    FeedSlider.DisplayInMapper = true;
                    ExtendLimitSlider.DisplayInMapper = true;
                    RetractLimitSlider.DisplayInMapper = true;
                    DampSlider.DisplayInMapper = false;
                    FeedSlider.SetRange(0f, 10f);
                    if (FeedSlider.Value > FeedSlider.Max) FeedSlider.SetValue(FeedSlider.Max);
                    //SC.SpringSlider can be null on stripped blocks in multiverse.
                    if (SpringSlider != null) SpringSlider.SetRange(0, 200f);
                    break;
            }
        }

        public void SuspensionMoveTowards(float target, float feed)
        {
            float movefeed = 0f;
            rigg.WakeUp();
            switch (selectedmovemode)
            {
                case 1: //Hydraulic   
                    movefeed = feed * 0.005f;
                    break;

                case 2: //Pneumatic
                    movefeed = feed * 20f;
                    break;
            }
                    CJ.targetPosition = Vector3.MoveTowards(CJ.targetPosition, new Vector3(target, 0, 0), movefeed);
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (rigg)
                {
                    if (SC.isSimulating)
                    {
                        if (selectedmovemode == 0)
                            return;

                        if (CJ)
                        {                           
                            if (isFirstFrame)
                            {                              
                                isFirstFrame = false;
                                JointDrive JD = CJ.xDrive;
                                //Debug.Log(JD.positionDamper);
                                switch (selectedmovemode)
                                {
                                    case 1: //Hydraulic    
                                        JD.positionDamper = Dampening - 12000;
                                        break;
                                    case 2: //Pneumatic
                                        JD.positionDamper = Dampening;
                                        break;
                                }
                                
                                CJ.xDrive = JD;
                            }
                                                      
                            float? target = null;

                            {
                                switch (selectedmovemode)
                                {
                                    case 1: //Hydraulic                                         
                                        if ((ExtendKey.IsHeld || ExtendKey.EmulationHeld()) && !RKpressed)
                                        {
                                            target = -ExtendLimit;
                                            
                                        }
                                        else
                                        {
                                            if (EKpressed)
                                            {
                                                if (RKpressed)
                                                    return;

                                                EKpressed = false;
                                                
                                            }
                                        }

                                        if ((RetractKey.IsHeld || RetractKey.EmulationHeld()) && !EKpressed)
                                        {
                                            target = RetractLimit;

                                            if (!RKpressed)
                                            {
                                                RKpressed = true;
                                                
                                            }
                                        }
                                        else
                                        {
                                            if (RKpressed)
                                            {
                                                if (EKpressed)
                                                    return;
                                                RKpressed = false;
                                                
                                            }
                                        }
                                        break;

                                    case 2: //Pneumatic
                                        if ((ExtendKey.IsHeld || ExtendKey.EmulationHeld()) && !RKpressed)
                                        {
                                            target = -ExtendLimit;
                                            if (!EKpressed)
                                            {
                                                EKpressed = true;
                                                
                                            }
                                        }
                                        else
                                        {
                                            if (EKpressed)
                                            {
                                                if (RKpressed)
                                                    return;
                                                EKpressed = false;
                                            }
                                        }

                                        if ((RetractKey.IsHeld || RetractKey.EmulationHeld()) && !EKpressed)
                                        {
                                            target = RetractLimit;

                                            if (!RKpressed)
                                            {
                                                RKpressed = true;
                                        
                                            }
                                        }
                                        else
                                        {
                                            if (RKpressed)
                                            {
                                                if (EKpressed)
                                                    return;
                                                RKpressed = false;                                              
                                            }
                                        }
                                        break;
                                }                               
                            }

                            if (target != null)
                            {
                                SuspensionMoveTowards((float)target, Feed);
                            }
                        }
                        else
                        {
                            if (HasBroken == false)
                            {
                                HasBroken = true;
                                StopSoundLoop();
                            }
                        }
                    }
                }
            }
        }

        public void StopSoundLoop()
        {
            //Debug.Log("AIRSOUNDLOOP STOP");
            Breaksound.pitch = Time.timeScale;
            Breaksound.Play();

            if (StatMaster.isClient || StatMaster.isLocalSim) return;
            ModNetworking.SendToAll(LS.CreateMessage(thisblock));
            //Debug.Log("Stop loop");
        }
        
        public static void StopLoopSoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0); 
            
            Pneumatics IO = BL.InternalObject.GetComponent<Pneumatics>();
            IO.Breaksound.Play();
            //Debug.Log("STOP SOUND");           
        }
    }
}