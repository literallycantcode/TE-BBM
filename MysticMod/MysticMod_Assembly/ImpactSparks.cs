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
    public class ImpactSparks : MonoBehaviour
    {

        public int floorLayer = 29;
        public BlockBehaviour BB;
        public ModTexture Sparktex = ModResource.GetTexture("sparkglow");
        public ParticleSystem Sparks;
        public static ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
        public int hugehitcount = 40;
        public int colskip = 1;
        public int flip = 0;
        public Vector3 Angle;
        public Vector3 place;
        public ContactPoint contact;
        public int Maxcol = 200;
    
        public void Awake()
        {
            Console.Log("Impact Sparks Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            this.initSparkFX();
        }

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
                        bool flag4 = sqrMagnitude > 10000f;
                        if (!flag4)
                        {
                            if (collisionInfo.impulse.sqrMagnitude <= 90000f)
                            {
                                this.Angle = gameObject.transform.eulerAngles;
								this.contact = collisionInfo.contacts[0];
								this.place = this.contact.point;
                                this.EmitSparks(this.place, this.Angle, true);
                                if (!StatMaster.isClient || StatMaster.isLocalSim)
                                {
                                    ModNetworking.SendToAll(Messages.emitbigsparks.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
                                }
                            }
                            else
                            {
								if (this.flip == this.colskip)
								{
									this.flip = 0;
								}
								else
								{
									this.flip++;
                                    this.Angle = gameObject.transform.eulerAngles;
									this.contact = collisionInfo.contacts[0];
									this.place = this.contact.point;
                                    this.EmitSparks(this.place, this.Angle, false);
                                    if (!StatMaster.isClient || StatMaster.isLocalSim)
                                    {
                                        ModNetworking.SendToAll(Messages.emitsmallsparks.CreateMessage(new object[] { this.place, this.Angle, this.BB }));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void EmitSparks(Vector3 contact, Vector3 angle, bool mode)
        {
            ImpactSparks.emitParams.position = contact;
            ImpactSparks.emitParams.startSize = UnityEngine.Random.Range(0.1f, 0.35f);
            ImpactSparks.emitParams.startColor = new Color32(byte.MaxValue, 90, 0, byte.MaxValue);
            ImpactSparks.emitParams.startLifetime = UnityEngine.Random.Range(0.1f, 2f);
            if (mode)
            {
                ImpactSparks.emitParams.applyShapeToPosition = true;
                this.Sparks.Emit(ImpactSparks.emitParams, this.hugehitcount);
            }
            else
            {
                ImpactSparks.emitParams.applyShapeToPosition = false;
                this.Sparks.Emit(ImpactSparks.emitParams, 1);
            }
        }

        public static void ProcessHugeHit(Message m)
		{
            Vector3 vector = (Vector3)m.GetData(0);
			Vector3 vector2 = (Vector3)m.GetData(1);
			Block block = (Block)m.GetData(2);
            ImpactSparks impactSparks = block.InternalObject.GetComponent<ImpactSparks>();
            bool flag = impactSparks == null;
			if (flag)
			{
				impactSparks = block.InternalObject.gameObject.AddComponent<ImpactSparks>();
			}
            impactSparks.EmitSparks(vector,vector2,true);
            Console.Log("HugeSparks Message Sent");
        }

        public static void ProcessSmallHit(Message m)
		{
            Vector3 vector = (Vector3)m.GetData(0);
			Vector3 vector2 = (Vector3)m.GetData(1);
			Block block = (Block)m.GetData(2);
            ImpactSparks impactSparks = block.InternalObject.GetComponent<ImpactSparks>();
            bool flag = impactSparks == null;
			if (flag)
			{
				impactSparks = block.InternalObject.gameObject.AddComponent<ImpactSparks>();
			}
            impactSparks.EmitSparks(vector,vector2,false);
            Console.Log("SmallSparks Message Sent");
        }
    }
}