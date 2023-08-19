using System;
using System.Linq;
using Modding;
using Modding.Blocks;
using static Modding.ModKeys;
using Console = Modding.ModConsole;
using UnityEngine;

namespace MysticFix
{
	public class Mod : ModEntryPoint
	{
		ModKey logKey; 

		public override void OnLoad()
		{
			// Called when the mod is loaded.
			this.logKey = ModKeys.GetKey("Logging");
			Console.Log("Hello world");
			SetupNetworking();
			OptionsMaster.defaultSmoothness = 0f;
			Physics.gravity = new Vector3(Physics.gravity.x, -55f, Physics.gravity.z);
			OptionsMaster.BesiegeConfig.MorePrecisePhysics = false;

			string[] frictionblocks={
				"Wheel",
				"LargeWheel",
				"CogLargeUnpowered",
				"Piston",
				"Hinge",
				"LargeWheelUnpowered"
			};

			string[] invtoggleblocks={
				"MetalBlade",
				"MetalSpike"
			};

			string[] invremoveblocks={
				"CogMediumPowered",
				"CircularSaw",
				"SpinningBlock"
			};

			Modding.Events.OnBlockInit += delegate(Block toInit)
            {
				Component[] configurablejoints;
				Component[] colliders;
				BlockBehaviour BB = toInit.InternalObject;
				GameObject block = BB.gameObject;
				Console.Log("Block init : '"+block.name+"'");

				//tick damage removal "borrowed" from Block Health Removal Tool
				if(BB.BlockHealth != null) {
                    if(block.name != "Crossbow") BB.BlockHealth.health = -1;
				    else BB.BlockHealth.health = Single.MaxValue;
					Console.Log("Tick damage removed");
				}
				if(BB.BreakOnImpact != null) {
                    BB.BreakOnImpact.reduceMultiplier = 0;
                    BB.BreakOnImpact.firstBreakForce = Single.MaxValue;
					Console.Log("break force multiplier removed");
                }
				if(BB.iceTag != null) {
					BB.iceTag.takesDamage = false;
					Console.Log("Freeze damage removed");}

				//starting to work on block tweaks
				if (!StatMaster.isClient || StatMaster.isLocalSim)
            	{
					switch(block.name)
					{
						case "GripPad":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Log":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "MetalBlade":
							BB.Prefab.myDamageType = DamageType.Blunt;
							block.GetComponent<Rigidbody>().mass=0.6f;
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							block.GetComponent<ConfigurableJoint>().breakForce=80000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=80000.0f;
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.12f;
								collider.material.dynamicFriction=0.12f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Spike":
							BB.Prefab.myDamageType = DamageType.Blunt;
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<ConfigurableJoint>().breakForce=80000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=80000.0f;
							colliders=block.GetComponentsInChildren<CapsuleCollider>();
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.12f;
								collider.material.dynamicFriction=0.12f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Wheel":
							block.GetComponent<HingeJoint>().breakForce=60000.0f;
							block.GetComponent<HingeJoint>().breakTorque=60000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Suspension":
							block.GetComponent<ConfigurableJoint>().breakForce=35000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=35000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "Piston":
							block.GetComponent<ConfigurableJoint>().breakForce=35000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=35000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;
						case "SmallWheel":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							block.GetComponent<HingeJoint>().breakForce=60000.0f;
							block.GetComponent<HingeJoint>().breakTorque=60000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "LargeWheel":
							block.GetComponent<HingeJoint>().breakForce=60000.0f;
							block.GetComponent<HingeJoint>().breakTorque=60000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;
						case "CogMediumPowered":
							
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "CogMediumUnpowered":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							block.GetComponent<HingeJoint>().breakForce=60000.0f;
							block.GetComponent<HingeJoint>().breakTorque=60000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "CogLargeUnpowered":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							block.GetComponent<HingeJoint>().breakForce=60000.0f;
							block.GetComponent<HingeJoint>().breakTorque=60000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "LargeWheelUnpowered":
							block.GetComponent<Rigidbody>().maxAngularVelocity=500;
							block.GetComponent<HingeJoint>().breakForce=55000.0f;
							block.GetComponent<HingeJoint>().breakTorque=55000.0f;
							
							Console.Log("Modified properties of: "+block.name);
							break;
						case "FlyingBlock":
							block.GetComponent<ConfigurableJoint>().breakForce=20000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=20000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Axle":
							configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
							foreach( ConfigurableJoint joint in configurablejoints )
							{
								joint.breakForce=60000.0f;
								joint.breakTorque=60000.0f;
							}
							Console.Log("Modified properties of: "+block.name);
							break;
						case "WingPanel":
							block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Swivel":
							block.GetComponent<HingeJoint>().breakForce=30000.0f;
							block.GetComponent<HingeJoint>().breakTorque=30000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Wing":
							block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Ballast":
							block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "MetalJaw":
							block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "SteeringBlock":
							block.GetComponent<ConfigurableJoint>().breakForce=60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=60000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "SteeringHinge":
							block.GetComponent<Rigidbody>().mass=0.4f;
							block.GetComponent<ConfigurableJoint>().breakForce=30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=30000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Hinge":
							block.GetComponent<ConfigurableJoint>().breakForce=35000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=35000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "BallJoint":
							block.GetComponent<ConfigurableJoint>().breakForce=30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=30000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "Decoupler":
							block.GetComponent<ConfigurableJoint>().breakForce=30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=30000.0f;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "CircularSaw":
							BB.Prefab.myDamageType = DamageType.Blunt;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "MetalBall":
							BB.Prefab.myDamageType = DamageType.Blunt;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "Propeller":
							BB.Prefab.myDamageType = DamageType.Blunt;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "SmallPropeller":
							BB.Prefab.myDamageType = DamageType.Blunt;
							Console.Log("Modified properties of: "+block.name);
							break;		
						case "SingleWoodenBlock":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
							foreach( ConfigurableJoint joint in configurablejoints )
							{
								joint.breakForce=60000.0f;
								joint.breakTorque=60000.0f;
							}
							Console.Log("Modified properties of: "+block.name);
							break;
						case "DoubleWoodenBlock":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
							foreach( ConfigurableJoint joint in configurablejoints )
							{
								joint.breakForce=50000.0f;
								joint.breakTorque=50000.0f;
							}
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "WoodenPole":
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
							foreach( ConfigurableJoint joint in configurablejoints )
							{
								joint.breakForce=40000.0f;
								joint.breakTorque=40000.0f;
							}
							Console.Log("Modified properties of: "+block.name);
							break;		
						case "StartingBlock":
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Plow":
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "HalfPipe":
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "ArmorPlateSmall":
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.1f;
								collider.material.dynamicFriction=0.1f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;		
						case "ArmorPlateLarge":
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.1f;
								collider.material.dynamicFriction=0.1f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;		
						case "ArmorPlateRound":
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.1f;
								collider.material.dynamicFriction=0.1f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "Cannon":
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "SpinningBlock":
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=5;
							Console.Log("Modified properties of: "+block.name);
							break;
						case "Grabber":
							block.GetComponent<Rigidbody>().mass=0.7f;
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0.01f;
							block.GetComponent<ConfigurableJoint>().breakForce=13750.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque=13750.0f;
							block.GetComponent<Rigidbody>().maxAngularVelocity=50;
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.01f;
								collider.material.dynamicFriction=0.01f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							Console.Log("Modified properties of: "+block.name);
							break;	
						case "WoodenPanel":
							block.GetComponent<Rigidbody>().mass=0.5f;
							block.GetComponent<Rigidbody>().angularDrag=0;
							block.GetComponent<Rigidbody>().drag=0;
							block.GetComponent<Rigidbody>().maxAngularVelocity=100;
							block.GetComponent<ConfigurableJoint>().breakForce=Mathf.Infinity;
							block.GetComponent<ConfigurableJoint>().breakTorque= Mathf.Infinity;
							colliders=block.GetComponentsInChildren<BoxCollider>();
							foreach(BoxCollider collider in colliders)
							{
								collider.material.staticFriction=0.1f;
								collider.material.dynamicFriction=0.1f;
								collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
							}
							Console.Log("Modified properties of: "+block.name);
							break;
					}
				}
				//apply friction controller
				if(frictionblocks.Contains(block.name))
				{
					if(block.GetComponent<FrictionController>()==null){block.AddComponent<FrictionController>();}
				}
				if(invtoggleblocks.Contains(block.name))
				{
					if(block.GetComponent<InvincibilityToggler>()==null){block.AddComponent<InvincibilityToggler>();}
				}
				if(invremoveblocks.Contains(block.name))
				{
					if(block.GetComponent<InvincibilityRemover>()==null){block.AddComponent<InvincibilityRemover>();}
				}
				if(block.name=="Grabber")
				{
					if(block.GetComponent<ExplosionStopper>()==null){block.AddComponent<ExplosionStopper>();}
					if(block.GetComponent<GrabberModifier>()==null){block.AddComponent<GrabberModifier>();}
				}
			};
			//Multiverse Cannonball tick damage removal 
			Modding.Events.OnConnect += delegate()
			{
				try
				{
					Component[] CannonBallTickDamagers = UnityEngine.GameObject.Find("PROJECTILES").GetComponentsInChildren<CannonBallDamage>(true);
					foreach (Component CBT in CannonBallTickDamagers)
					{
						UnityEngine.Object.Destroy(CBT);
					}
				}
				catch(NullReferenceException)
				{
					ModConsole.Log("Could not find GameObject PROJECTILES when logging into a multiverse session.");
				}
			};
		}

		public void SetupNetworking()
        	{
					
			}
		
	}
}
