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
  public BlockBehaviour BB;
  public ModTexture Sparktex = ModResource.GetTexture("sparkglow");
  public ParticleSystem Sparks;
  public static ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
  public int floorLayer = 29;
  public AudioSource Hitsound;
  public AudioSource SmallHitsound;
  public List<AudioClip> HH = new List<AudioClip>();
  public List<AudioClip> SH = new List<AudioClip>();
  public int hugehitcount = 40;
  public int colskip = 1;
  public int flip = 0;
  public Vector3 Angle;
  public Vector3 place;
  public ContactPoint contact;
  public int Maxcol = 200;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Awake function
    public void Awake()
        {
            Console.Log("Impact Effects Added to "+gameObject.name);
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
			ImpactEffects sparkScript = block.InternalObject.GetComponent<ImpactEffects>();
			if (sparkScript == null)
			{
				sparkScript = block.InternalObject.gameObject.AddComponent<ImpactEffects>();
			}
			sparkScript.EmitSparks(vector, vector2, false);
		}
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Process Huge Hit
        public static void ProcessHugeHit(Message m)
		{
			Vector3 vector = (Vector3)m.GetData(0);
			Vector3 vector2 = (Vector3)m.GetData(1);
			Block block = (Block)m.GetData(2);
			ImpactEffects sparkScript = block.InternalObject.GetComponent<ImpactEffects>();
			bool flag = sparkScript == null;
			if (flag)
			{
				sparkScript = block.InternalObject.gameObject.AddComponent<ImpactEffects>();
			}
			sparkScript.EmitSparks(vector, vector2, true);
		}
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////Spark Emission
        public void EmitSparks(Vector3 contact, Vector3 angle, bool mode)
        {
            ImpactEffects.emitParams.position = contact;
            ImpactEffects.emitParams.startSize = UnityEngine.Random.Range(0.1f, 0.35f);
            ImpactEffects.emitParams.startColor = new Color32(byte.MaxValue, 90, 0, byte.MaxValue);
            ImpactEffects.emitParams.startLifetime = UnityEngine.Random.Range(0.1f, 2f);
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
                ImpactEffects.emitParams.applyShapeToPosition = true;
                this.Sparks.Emit(ImpactEffects.emitParams, this.hugehitcount);
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
                ImpactEffects.emitParams.applyShapeToPosition = false;
                this.Sparks.Emit(ImpactEffects.emitParams, 1);
            }
        }
    }
}
