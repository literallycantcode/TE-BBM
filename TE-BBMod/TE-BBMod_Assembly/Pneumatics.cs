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
        private MMenu SoundMenu;
        private MMenu SoundMode;
        private MToggle HydraSound;
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
        private int selectedsound;
        private int soundmode;

        private bool Break = false;
        public bool HSound;
        private bool HasBroken = false;
        private bool EKpressed = false;
        private bool RKpressed = false;
        private bool isFirstFrame = true;

        List<AudioClip> ASounds = new List<AudioClip>()
        {
            null,
            ModResource.GetAudioClip("air1"),
            ModResource.GetAudioClip("air2"),
            ModResource.GetAudioClip("air3"),
            ModResource.GetAudioClip("air4"),
            ModResource.GetAudioClip("air5"),
            ModResource.GetAudioClip("air6"),
            ModResource.GetAudioClip("air7"),
            ModResource.GetAudioClip("air8"),
            ModResource.GetAudioClip("air9"),
            ModResource.GetAudioClip("air10"),
            ModResource.GetAudioClip("air11"),
            ModResource.GetAudioClip("air12"),
            ModResource.GetAudioClip("air13"),
        };

        private AudioSource Airsound;
        private AudioSource Stopsound;
        private AudioSource Breaksound;

        internal static List<string> MoveModes = new List<string>()
        {
            "None",
            "Hydraulic",
            "Pneumatic",
        };

        internal static List<string> Soundmode = new List<string>()
        {
            "on extend",
            "on retract",
            "on both",
        };

        internal static List<string> FireSound = new List<string>()
        {
            "None",
            "TUNGTSS",
            "SSAAAA_TSHUH",
            "PSHH",
            "BSUH",
            "TSEE",
            "TKSHHHH",
            "TSSSIT",
            "TUUIIY",
            "PFFFUUUI",
            "PFUUT",
            "TFUU",
            "TFOOINGING",
            "JUDGEBURST",
        };

        internal static MessageType A1;
        internal static MessageType LP;
        internal static MessageType LS;


        internal static void SetupNetworking()
        {
            A1 = ModNetworking.CreateMessageType(DataType.Block, DataType.Integer);
            LP = ModNetworking.CreateMessageType(DataType.Block);
            LS = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[A1] += PlaySoundClient;
            ModNetworking.Callbacks[LP] += PlayLoopSoundClient;
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
                            CJ.breakForce = 60000;
                            CJ.breakTorque = 60000;
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

            HydraSound = SC.AddToggle("UseSound", "Hydra Sound", HSound);
            HydraSound.Toggled += (bool value) => { HSound = value; };

            SoundMenu = SC.AddMenu("Firesound", selectedsound, FireSound, false);
            SoundMenu.ValueChanged += (ValueHandler)(value => { selectedsound = value; });

            SoundMode = SC.AddMenu("SoundMode", soundmode, Soundmode, false);
            SoundMode.ValueChanged += (ValueHandler)(value => { soundmode = value; });

            //DisplayInMapper config
            MoveMode.DisplayInMapper = true;

            Airsound = SC.gameObject.AddComponent<AudioSource>();
            Airsound.rolloffMode = AudioRolloffMode.Linear;
            Airsound.spatialBlend = 1;
            Airsound.playOnAwake = false;

            switch (selectedmovemode)
            {
                case 0: //None
                    break;

                case 1: //Hydraulic
                    Airsound.clip = ModResource.GetAudioClip("bigmotor_loop");
                    Airsound.loop = true;
                    Airsound.maxDistance = 100f;
                    Airsound.volume = 0.2f;
                    Airsound.pitch = Feed / 8;
                    break;

                case 2: //Pneumatic
                    Airsound.clip = ASounds[selectedsound];
                    Airsound.loop = false;
                    Airsound.maxDistance = 150f;
                    Airsound.volume = 0.3f;
                    break;
            }
            
            Stopsound = SC.gameObject.AddComponent<AudioSource>();
            Stopsound.spatialBlend = 1;
            Stopsound.maxDistance = 150f;
            Stopsound.rolloffMode = AudioRolloffMode.Linear;
            Stopsound.volume = 0.3f;
            Stopsound.playOnAwake = false;
            Stopsound.clip = ModResource.GetAudioClip("bigmotor_Stop");
            Stopsound.loop = false;
            Stopsound.pitch = Feed / 8;

            Breaksound = SC.gameObject.AddComponent<AudioSource>();
            Breaksound.spatialBlend = 1;
            Breaksound.maxDistance = 150f;
            Breaksound.rolloffMode = AudioRolloffMode.Linear;
            Breaksound.volume = 0.3f;
            Breaksound.playOnAwake = false;
            Breaksound.clip = ModResource.GetAudioClip("air_Broken");
            
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
                    HydraSound.DisplayInMapper = false;
                    SoundMenu.DisplayInMapper = false;
                    SoundMode.DisplayInMapper = false;
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
                    DampSlider.DisplayInMapper = true;
                    HydraSound.DisplayInMapper = true;
                    SoundMenu.DisplayInMapper = false;
                    SoundMode.DisplayInMapper = false;
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
                    DampSlider.DisplayInMapper = true;
                    HydraSound.DisplayInMapper = false;
                    SoundMenu.DisplayInMapper = true;
                    SoundMode.DisplayInMapper = true;
                    HSound = false;
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
                                            if (!EKpressed)
                                            {
                                                EKpressed = true;                                              
                                                if (HSound)
                                                {
                                                    PlaySoundLoop();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (EKpressed)
                                            {
                                                if (RKpressed)
                                                    return;

                                                EKpressed = false;
                                                if (HSound)
                                                {
                                                    StopSoundLoop(false);
                                                }
                                            }
                                        }

                                        if ((RetractKey.IsHeld || RetractKey.EmulationHeld()) && !EKpressed)
                                        {
                                            target = RetractLimit;

                                            if (!RKpressed)
                                            {
                                                RKpressed = true;
                                                if (HSound)
                                                {
                                                    PlaySoundLoop();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (RKpressed)
                                            {
                                                if (EKpressed)
                                                    return;
                                                RKpressed = false;
                                                if (HSound)
                                                {
                                                    StopSoundLoop(false);
                                                }
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
                                                if (soundmode == 0 || soundmode == 2)
                                                {
                                                    PlaySound();
                                                }
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
                                                if (soundmode == 1 || soundmode == 2)
                                                {
                                                    PlaySound();
                                                }
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
                                StopSoundLoop(true);
                            }
                        }
                    }
                }
            }
        }

        private void PlaySound()
        {
            if (ASounds[selectedsound] == null)
                return;
            this.Airsound.clip = ASounds[selectedsound];
            this.Airsound.pitch = Time.timeScale;
            this.Airsound.Play();

            if (StatMaster.isClient || StatMaster.isLocalSim)
                return;
            ModNetworking.SendToAll(A1.CreateMessage(thisblock,selectedsound));
        }

        private void PlaySoundLoop()
        {
            this.Airsound.clip = ModResource.GetAudioClip("bigmotor_loop");
            this.Airsound.Play();

            if (StatMaster.isClient || StatMaster.isLocalSim)
                return;
            ModNetworking.SendToAll(LP.CreateMessage(thisblock));
            //Debug.Log("Play loop");
        }

        public void StopSoundLoop(bool broke)
        {
            //Debug.Log("AIRSOUNDLOOP STOP");
            this.Airsound.Stop();

            if (broke)
            {
                Breaksound.pitch = Time.timeScale;
                Breaksound.Play();
            }
            else
            {
                Stopsound.Play();               
            }

            if (StatMaster.isClient || StatMaster.isLocalSim)
                return;
            ModNetworking.SendToAll(LS.CreateMessage(thisblock, broke));
            //Debug.Log("Stop loop");
        }

        public static void PlaySoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            int sound = (int)m.GetData(1);
            BL.InternalObject.GetComponent<Pneumatics>().Airsound.clip = BL.InternalObject.GetComponent<Pneumatics>().ASounds[sound];
            BL.InternalObject.GetComponent<Pneumatics>().Airsound.Play();
            //Debug.Log("Playsound");
        }

        public static void PlayLoopSoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            BL.InternalObject.GetComponent<Pneumatics>().Airsound.clip = ModResource.GetAudioClip("bigmotor_loop");
            BL.InternalObject.GetComponent<Pneumatics>().Airsound.Play();
            //Debug.Log("Playsound LOOP");
        }

        public static void StopLoopSoundClient(Message m)
        {
            Block BL = (Block)m.GetData(0);
            
            Pneumatics IO = BL.InternalObject.GetComponent<Pneumatics>();
            IO.Break = (bool)m.GetData(1);
            IO.Airsound.clip = ModResource.GetAudioClip("bigmotor_Stop");
            IO.Airsound.Stop();
            //Debug.Log("STOP SOUND");

            if (IO.HSound) { IO.Stopsound.Play(); }
            if (IO.Break) { IO.Breaksound.Play(); }           
        }
    }
}