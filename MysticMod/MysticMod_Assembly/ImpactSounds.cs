using System;
using System.Linq;
using Modding;
using Modding.Blocks;
using static Modding.ModKeys;
using System.Collections.Generic;
using Console = Modding.ModConsole;
using UnityEngine;

namespace MysticFix
{
    public class ImpactSounds : MonoBehaviour
    {
        public bool set = false;
        public int floorLayer = 29;
        public AudioSource Hitsound;
        public AudioSource SmallHitsound;
        public List<AudioClip> HH = new List<AudioClip>();
        public List<AudioClip> SH = new List<AudioClip>();
        public BlockBehaviour BB;
        public void Awake()
        {
            Console.Log("Impact Sounds Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            bool flag = !this.BB.isSimulating;
			if (!flag)
			{
                this.Hitsound = this.BB.gameObject.AddComponent<AudioSource>();
				this.HH.Add(Soundfiles.hh0);
				this.HH.Add(Soundfiles.hh1);
				this.HH.Add(Soundfiles.hh2);
				this.HH.Add(Soundfiles.hh3);
				this.HH.Add(Soundfiles.hh4);
				this.HH.Add(Soundfiles.hh5);
				this.HH.Add(Soundfiles.hh6);
				this.HH.Add(Soundfiles.hh7);
				this.HH.Add(Soundfiles.hh8);
				this.HH.Add(Soundfiles.hh9);
				this.Hitsound.spatialBlend = 1f;
				this.Hitsound.maxDistance = 400f;
				this.Hitsound.rolloffMode = AudioRolloffMode.Linear;
				this.Hitsound.volume = 0.5f;
				this.Hitsound.playOnAwake = false;
				this.Hitsound.loop = false;
				this.SmallHitsound = this.BB.gameObject.AddComponent<AudioSource>();
				this.SH.Add(Soundfiles.sh1);
				this.SH.Add(Soundfiles.sh2);
				this.SH.Add(Soundfiles.sh3);
				this.SH.Add(Soundfiles.sh4);
				this.SH.Add(Soundfiles.sh5);
				this.SH.Add(Soundfiles.sh6);
				this.SH.Add(Soundfiles.sh7);
				this.SmallHitsound.spatialBlend = 1f;
				this.SmallHitsound.maxDistance = 200f;
				this.SmallHitsound.rolloffMode = AudioRolloffMode.Linear;
				this.SmallHitsound.volume = 0.07f;
				this.SmallHitsound.playOnAwake = false;
				this.SmallHitsound.loop = false;
                Console.Log("Audio set up");
            }
        }
        
        public void OnCollisionEnter(Collision collisionInfo)
		{
            bool flag = !this.BB.isSimulating || !this.BB.SimPhysics || collisionInfo == null || collisionInfo.rigidbody == null;
            if(!flag)
            {
                GameObject gameObject = collisionInfo.gameObject;
                float sqrMagnitude = collisionInfo.relativeVelocity.sqrMagnitude;
                bool flag2 = gameObject.layer == this.floorLayer;
				if (!flag2)
				{
                    bool flag3 = sqrMagnitude > 300000f;
					if (!flag3)
					{
                        bool flag4 = sqrMagnitude < 10000f;
                        if (!flag4)
                        {
                            if (collisionInfo.impulse.sqrMagnitude <= 90000f)
                            {
                                playImpactSound(true);
                                if (!StatMaster.isClient || StatMaster.isLocalSim)
                                {
                                    ModNetworking.SendToAll(Messages.playbigsound.CreateMessage(this.BB));
                                }
                            }
                            else
                            {
                                playImpactSound(false);
                                if (!StatMaster.isClient || StatMaster.isLocalSim)
                                {
                                    ModNetworking.SendToAll(Messages.playsmallsound.CreateMessage(this.BB));
                                }
                            }
                        }
                    }
                }
            }
        }

        public void playImpactSound(bool big)
        {
            if(big)
            {
                    int num = UnityEngine.Random.Range(0, this.HH.Count);
                    this.Hitsound.clip = this.HH[num];
                    this.Hitsound.pitch = Time.timeScale;
                    this.Hitsound.Play();
            }
            else
            {
                    int num2 = UnityEngine.Random.Range(0, this.SH.Count);
                    this.Hitsound.clip = this.SH[num2];
                    this.Hitsound.pitch = Time.timeScale;
                    this.Hitsound.Play();
            }
        }

        public static void ProcessHugeHit(Message m)
		{
            Block block = (Block)m.GetData(0);
            ImpactSounds impactSounds = block.InternalObject.GetComponent<ImpactSounds>();
            bool flag = impactSounds == null;
			if (flag)
			{
				impactSounds = block.InternalObject.gameObject.AddComponent<ImpactSounds>();
			}
            impactSounds.playImpactSound(true);
            Console.Log("HugeHit Message Sent");
        }

        public static void ProcessSmallHit(Message m)
		{
            Block block = (Block)m.GetData(0);
            ImpactSounds impactSounds = block.InternalObject.GetComponent<ImpactSounds>();
            bool flag = impactSounds == null;
			if (flag)
			{
				impactSounds = block.InternalObject.gameObject.AddComponent<ImpactSounds>();
			}
            impactSounds.playImpactSound(false);
            Console.Log("SmallHit Message Sent");
        }
    }
}