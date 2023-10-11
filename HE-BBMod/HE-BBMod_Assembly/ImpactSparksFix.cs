//this is an attempt to roll all 3 impact codes together into 1 and improve performance
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
  public class ImpactSparksFix : MonoBehaviour
  {	
  	////taken from ImpactEffects
 	public BlockBehaviour BB;
	public ModTexture Sparktex = ModResource.GetTexture("sparkglow");
	public ParticleSystem Sparks;
	public static ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
	public AudioSource Hitsound;
	public AudioSource SmallHitsound;
  	public List<AudioClip> HH = new List<AudioClip>();
  	public List<AudioClip> SH = new List<AudioClip>();
	public int hugehitcount = 35;
  	//public int colskip = 1;
  	//public int flip = 0;
  	public Vector3 Angle;
  	public Vector3 place;
  	public ContactPoint contact;
  	public int Maxcol = 200;
	////taken from ImpactSparks
 	public int cooldown = 0;
  	
 	////taken from ImpactSounds

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Awake function
    public void Awake()
        {
            Console.Log("Impact Sparks Fix "+gameObject.name);
            this.BB = base.GetComponent<BlockBehaviour>();
            this.initSparkFX();
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
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Collision Detect
	
	 public void OnCollisionEnter(Collision collisionInfo)
		{
            if(!this.BB.isSimulating || !this.BB.SimPhysics || collisionInfo == null || collisionInfo.rigidbody == null)
            {
                GameObject gameObject = collisionInfo.gameObject;
                float sqrMagnitude = collisionInfo.relativeVelocity.sqrMagnitude;
					
			if (collisionInfo.impulse.sqrMagnitude >= 20000f && collisionInfo.impulse.sqrMagnitude <= 90000f)
				{
					    Console.Log("Small Hit");
	 				    //this.flip++;
                                            this.Angle = gameObject.transform.eulerAngles;
                                            this.contact = collisionInfo.contacts[0];
                                            this.place = this.contact.point;
                                           
					    this.EmitSparks(this.place, this.Angle, false);
                                            if (!StatMaster.isClient || StatMaster.isLocalSim)
                                            {
                                                
						ModNetworking.SendToAll(Messages.emitsmallsparks.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
                                            }	

	 				this.EmitSparks(this.place, this.Angle, false);
					bool flag8 = !StatMaster.isMP || StatMaster.isClient;
						if (!flag8)
						{
						bool sendmess2 = Soundfiles.sendmess;
						if (sendmess2)
						{
						ModNetworking.SendInSimulation(Messages.col.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
						}
						}

      					ModNetworking.SendToAll(Messages.playsmallsound.CreateMessage(this.BB))		
				}
			if  (collisionInfo.impulse.sqrMagnitude >= 90000f)
				{
       				this.contact = collisionInfo.contacts[0];
	   			 Console.Log("Big Hit");
				this.place = this.contact.point;
                               
				this.EmitSparks(this.place, this.Angle, true);
				bool flag6 = !StatMaster.isMP || StatMaster.isClient;
				if (!flag6)
					{
					bool sendmess = Soundfiles.sendmess;
				if (sendmess)
					{
					ModNetworking.SendToAll(Messages.hugehit.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
					}
     					}
	  
	  			 this.EmitSparks(this.place, this.Angle, true);
                                        if (!StatMaster.isClient || StatMaster.isLocalSim)
                                        {
                                            ModNetworking.SendToAll(Messages.emitbigsparks.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
                                        }
	  				
                                	ModNetworking.SendToAll(Messages.playbigsound.CreateMessage(this.BB));
                                        
				}
    		}
      }
	
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Initialize Sparks FX
 	    public void initSparkFX()
		{
			bool flag = this.BB.GetComponent<ParticleSystem>();
			if (!flag)
			{
				this.Sparks = this.BB.gameObject.AddComponent<ParticleSystem>();
				Material material = new Material(Shader.Find("Particles/Additive"));
				material.mainTexture = this.Sparktex;
				ParticleSystemRenderer component = this.Sparks.GetComponent<ParticleSystemRenderer>();
				component.material = material;
				component.renderMode = ParticleSystemRenderMode.Stretch;
				component.velocityScale = 0.03f;
				component.lengthScale = 3f;
				ParticleSystem sparks = this.Sparks;
				ParticleSystem.EmissionModule emission = this.Sparks.emission;
				ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime = this.Sparks.limitVelocityOverLifetime;
				ParticleSystem.CollisionModule collision = this.Sparks.collision;
				ParticleSystem.ColorOverLifetimeModule colorOverLifetime = this.Sparks.colorOverLifetime;
				ParticleSystem.ShapeModule shape = this.Sparks.shape;
				ParticleSystem.InheritVelocityModule inheritVelocity = this.Sparks.inheritVelocity;
				sparks.playOnAwake = false;
				sparks.loop = false;
				sparks.maxParticles = 500;
				sparks.startSpeed = 100f;
				sparks.gravityModifier = 1f;
				sparks.simulationSpace = ParticleSystemSimulationSpace.World;
				emission.rate = 200f;
				emission.enabled = false;
				limitVelocityOverLifetime.enabled = true;
				limitVelocityOverLifetime.dampen = 0.05f;
				collision.enabled = true;
				collision.type = ParticleSystemCollisionType.World;
				collision.mode = ParticleSystemCollisionMode.Collision3D;
				collision.dampen = 0.1f;
				collision.bounce = 0f;
				collision.radiusScale = 2f;
				collision.maxCollisionShapes = 200;
				shape.shapeType = ParticleSystemShapeType.Sphere;
				shape.radius = 0.2f;
				colorOverLifetime.enabled = true;
				inheritVelocity.enabled = true;
				Gradient gradient = new Gradient();
				gradient.SetKeys(new GradientColorKey[]
				{
					new GradientColorKey(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue), 0f)
				}, new GradientAlphaKey[]
				{
					new GradientAlphaKey(1f, 0f),
					new GradientAlphaKey(1f, 0.5f),
					new GradientAlphaKey(0f, 1f)
				});
				colorOverLifetime.color = gradient;
			}
		}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Process Small Hit
          public static void ProcessSmallHit(Message m)
		{
			Vector3 vector = (Vector3)m.GetData(0);
			Vector3 vector2 = (Vector3)m.GetData(1);
			Block block = (Block)m.GetData(2);
			ImpactSparksFix sparkScript = block.InternalObject.GetComponent<ImpactSparksFix>();
			if (sparkScript == null)
			{
				sparkScript = block.InternalObject.gameObject.AddComponent<ImpactSparksFix>();
			}
			sparkScript.EmitSparks(vector, vector2, false);
		}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Process Huge Hit
        public static void ProcessHugeHit(Message m)
		{
			Vector3 vector = (Vector3)m.GetData(0);
			Vector3 vector2 = (Vector3)m.GetData(1);
			Block block = (Block)m.GetData(2);
			ImpactSparksFix sparkScript = block.InternalObject.GetComponent<ImpactSparksFix>();
			bool flag = sparkScript == null;
			if (flag)
			{
				sparkScript = block.InternalObject.gameObject.AddComponent<ImpactSparksFix>();
			}
			sparkScript.EmitSparks(vector, vector2, true);
		}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Spark Emission
        public void EmitSparks(Vector3 contact, Vector3 angle, bool mode)
        {
            ImpactSparksFix.emitParams.position = contact;
            ImpactSparksFix.emitParams.startSize = UnityEngine.Random.Range(0.1f, 0.35f);
            ImpactSparksFix.emitParams.startColor = new Color32(byte.MaxValue, 90, 0, byte.MaxValue);
            ImpactSparksFix.emitParams.startLifetime = UnityEngine.Random.Range(0.1f, 2f);
            if (mode)
            {
                bool flag = !Soundfiles.mute;
                if (flag)
                {
                    int num = UnityEngine.Random.Range(0, this.HH.Count);
                    this.Hitsound.clip = this.HH[num];
                    this.Hitsound.pitch = Time.timeScale;
                    this.Hitsound.Play();
                }
                ImpactSparksFix.emitParams.applyShapeToPosition = true;
                this.Sparks.Emit(ImpactSparksFix.emitParams, this.hugehitcount);
            }
            else
            {
                bool flag3 = !Soundfiles.mute;
                if (flag3)
                {
                    int num2 = UnityEngine.Random.Range(0, this.SH.Count);
                    this.SmallHitsound.clip = this.SH[num2];
                    this.SmallHitsound.pitch = Time.timeScale;
                    this.SmallHitsound.Play();
                }
                ImpactSparksFix.emitParams.applyShapeToPosition = false;
                this.Sparks.Emit(ImpactSparksFix.emitParams, 1);
            }
        }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// impact sounds
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
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////		
    }
}

