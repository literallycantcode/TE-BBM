using System;
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

			Modding.Events.OnBlockInit += delegate(Block toInit)
            {
				Component[] configurablejoints;
				Component[] colliders;
				BlockBehaviour BB = toInit.InternalObject;
				GameObject block = BB.gameObject;
				Console.Log("Block init :"+block.name);

				//tick damage removal "borrowed" from Block Health Removal Tool
				if(BB.BlockHealth != null) {
                    if(block.name != "Crossbow") BB.BlockHealth.health = -1;
				    else BB.BlockHealth.health = Single.MaxValue;
				}
				if(BB.BreakOnImpact != null) {
                    BB.BreakOnImpact.reduceMultiplier = 0;
                    BB.BreakOnImpact.firstBreakForce = Single.MaxValue;
                }
				if(BB.iceTag != null) BB.iceTag.takesDamage = false;

				//starting to work on block tweaks
				switch(block.name)
				{
					case "GripPad":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;
					case "MetalBlade":
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
						break;
					case "Spike":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<ConfigurableJoint>().breakForce=80000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=80000.0f;
						colliders=block.GetComponentsInChildren<BoxCollider>();
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						foreach(BoxCollider collider in colliders)
						{
							collider.material.staticFriction=0.12f;
							collider.material.dynamicFriction=0.12f;
							collider.material.frictionCombine=PhysicMaterialCombine.Minimum;
						}
						break;
					case "Wheel":
						block.GetComponent<HingeJoint>().breakForce=60000.0f;
						block.GetComponent<HingeJoint>().breakTorque=60000.0f;
						block.AddComponent<FrictionController>();
						break;
					case "Suspension":
						block.GetComponent<HingeJoint>().breakForce=35000.0f;
						block.GetComponent<HingeJoint>().breakTorque=35000.0f;
						break;	
					case "Piston":
						block.GetComponent<HingeJoint>().breakForce=35000.0f;
						block.GetComponent<HingeJoint>().breakTorque=35000.0f;
						block.AddComponent<FrictionController>();
						break;
					case "SmallWheel":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						block.GetComponent<HingeJoint>().breakForce=60000.0f;
						block.GetComponent<HingeJoint>().breakTorque=60000.0f;
						break;	
					case "LargeWheel":
						block.GetComponent<HingeJoint>().breakForce=60000.0f;
						block.GetComponent<HingeJoint>().breakTorque=60000.0f;
						block.AddComponent<FrictionController>();
						break;
					case "CogMediumUnpowered":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						block.GetComponent<HingeJoint>().breakForce=60000.0f;
						block.GetComponent<HingeJoint>().breakTorque=60000.0f;
						block.AddComponent<FrictionController>();
						break;	
					case "CogLargeUnpowered":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						block.GetComponent<HingeJoint>().breakForce=60000.0f;
						block.GetComponent<HingeJoint>().breakTorque=60000.0f;
						block.AddComponent<FrictionController>();
						break;	
					case "LargeWheelUnpowered":
						block.GetComponent<Rigidbody>().maxAngularVelocity=500;
						block.GetComponent<HingeJoint>().breakForce=55000.0f;
						block.GetComponent<HingeJoint>().breakTorque=55000.0f;
						block.AddComponent<FrictionController>();
						break;
					case "FlyingBlock":
						block.GetComponent<ConfigurableJoint>().breakForce=20000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=20000.0f;
						break;
					case "Axle":
						configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
						foreach( ConfigurableJoint joint in configurablejoints )
						{
							joint.breakForce=60000.0f;
							joint.breakTorque=60000.0f;
						}
						break;
					case "WingPanel":
						block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
						break;
					case "Ballast":
						block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
						break;
					case "MetalJaw":
						block.GetComponent<ConfigurableJoint>().breakForce=50000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=50000.0f;
						break;	
					case "SteeringBlock":
						block.GetComponent<ConfigurableJoint>().breakForce=60000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=60000.0f;
						break;
					case "SteeringHinge":
						block.GetComponent<ConfigurableJoint>().breakForce=30000.0f;
						block.GetComponent<ConfigurableJoint>().breakTorque=30000.0f;
						break;	
					case "SingleWoodenBlock":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
						foreach( ConfigurableJoint joint in configurablejoints )
						{
							joint.breakForce=60000.0f;
							joint.breakTorque=60000.0f;
						}
						break;
					case "DoubleWoodenBlock":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
						foreach( ConfigurableJoint joint in configurablejoints )
						{
							joint.breakForce=50000.0f;
							joint.breakTorque=50000.0f;
						}
						break;	
					case "WoodenPole":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						configurablejoints=block.GetComponentsInChildren<ConfigurableJoint>();
						foreach( ConfigurableJoint joint in configurablejoints )
						{
							joint.breakForce=40000.0f;
							joint.breakTorque=40000.0f;
						}
						break;		
					case "StartingBlock":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;
					case "Plow":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;	
					case "HalfPipe":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;	
					case "ArmorPlateSmall":
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;		
					case "ArmorPlateLarge":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;		
					case "ArmorPlateRound":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;	
					case "Cannon":
						block.GetComponent<Rigidbody>().angularDrag=0;
						block.GetComponent<Rigidbody>().drag=0;
						block.GetComponent<Rigidbody>().maxAngularVelocity=100;
						break;
					case "Grabber":
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
						break;	
					case "WoodenPanel":
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
						break;
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
		
	}
}
