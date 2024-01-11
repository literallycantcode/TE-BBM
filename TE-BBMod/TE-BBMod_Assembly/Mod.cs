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

			SetupNetworking();

			string[] frictionblocks ={
				"Wheel",
				"LargeWheel",
				"CogLargeUnpowered",
				"Piston",
				"Hinge",
				"WheelUnpowered",
				"LargeWheelUnpowered",
				"GripPad"
			};

			string[] dragfixblocks =
			{
				"Wing",
				"WingPanel",
			};

			string[] invtoggleblocks ={
				"MetalBlade",
				"Spike"
			};

			string[] roundwheelblocks ={
				"Wheel",
				"LargeWheel",
				"WheelUnpowered",
				"LargeWheelUnpowered"
			};

			string[] spinuptimeblocks ={
				"Wheel",
				"LargeWheel",
				"CogMediumPowered"
			};


			string[] invremoveblocks ={
				"CogMediumPowered",
				"CircularSaw",
				"SpinningBlock"
			};

			string[] smokefixblocks ={
				"Log",
				"SingleWoodenBlock",
				"DoubleWoodenBlock"
			};

			string[] sfxblocks ={
				"StartingBlock",
				"MetalBlade",
				"Decoupler",
				"MetalBall",
				"Cannon",
				"ScalingBlock",
				"SteeringBlock",
				"Suspension",
				"Suspension",
				"Piston",
				"Swivel",
				"Spike",
				"SpinningBlock",
				"ArmorPlateSmall",
				"Grabber",
				"SteeringHinge",
				"BombHolder",
				"ArmorPlateLarge",
				"Plow",
				"Ballast",
				"HalfPipe",
				"BallJoint",
				"Torch",
				"Drill",
				"ShrapnelCannon",
				"WaterCannon",
				"Vacuum",
				"Altimeter",
				"Anglometer",
				"LogicGate",
				"Sensor",
				"Speedometer"
			};
			Modding.Events.OnBlockInit += delegate (Block toInit)
			{
				Component[] configurablejoints;
				Component[] colliders;
				BlockBehaviour BB = toInit.InternalObject;
				GameObject block = BB.gameObject;
				Console.Log("Block init : '" + block.name + "'");

				//tick damage removal "borrowed" from Block Health Removal Tool
				if (BB.BlockHealth != null)
				{
					if (block.name != "Crossbow") BB.BlockHealth.health = -1;
					else BB.BlockHealth.health = Single.MaxValue;
					Console.Log("Tick damage removed");
				}
				if (BB.BreakOnImpact != null)
				{
					BB.BreakOnImpact.reduceMultiplier = 0;
					BB.BreakOnImpact.firstBreakForce = Single.MaxValue;
					Console.Log("break force multiplier removed");
				}
				if (BB.iceTag != null)
				{
					BB.iceTag.takesDamage = false;
					Console.Log("Freeze damage removed");
				}
				//starting to work on block tweaks
				if (!StatMaster.isClient || StatMaster.isLocalSim)
				{
					if (BB.SimPhysics)
					{
						if (block.GetComponent<DelayedTweaker>() == null) { block.AddComponent<DelayedTweaker>(); }
					}

					switch (block.name)
					{
						/*
						case "Drill":
						case "WheelUnpowered":
						case "ShrapnelCannon":
						case "Torch":
						case "GripPad":
						case "Log":
						case "CogMediumUnpowered":
						case "StartingBlock":
						case "Plow":
						case "HalfPipe":
						case "Cannon":
						case "WaterCannon":
							Console.Log("Did Nothing To: " + block.name);
							break;
						*/

						case "MetalBlade":
							BB.Prefab.myDamageType = DamageType.Blunt;
							block.GetComponent<Rigidbody>().mass = 0.6f;
							block.GetComponent<ConfigurableJoint>().breakForce = 80000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 80000.0f;
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Spike":
							BB.Prefab.myDamageType = DamageType.Blunt;
							block.GetComponent<Rigidbody>().mass = 0.6f;
							block.GetComponent<ConfigurableJoint>().breakForce = 80000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 80000.0f;
							colliders = block.GetComponentsInChildren<CapsuleCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Wheel":
							block.GetComponent<HingeJoint>().breakForce = 60000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Suspension":
							block.GetComponent<ConfigurableJoint>().breakForce = 35000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 35000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Piston":
							block.GetComponent<ConfigurableJoint>().breakForce = 35000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 35000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SmallWheel":
							block.GetComponent<HingeJoint>().breakForce = 60000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "LargeWheel":
							block.GetComponent<HingeJoint>().breakForce = 70000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 70000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "CogMediumPowered":
							block.GetComponent<HingeJoint>().breakForce = 90000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 90000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						
						case "CogLargeUnpowered":
							block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "LargeWheelUnpowered":
							block.GetComponent<HingeJoint>().breakForce = 60000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "FlyingBlock":
							block.GetComponent<ConfigurableJoint>().breakForce = 20000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 20000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Axle":
							configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
							foreach (ConfigurableJoint joint in configurablejoints)
							{
								joint.breakForce = 60000.0f;
								joint.breakTorque = 60000.0f;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "WingPanel":
							block.GetComponent<ConfigurableJoint>().breakForce = 40000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 40000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Swivel":
							block.GetComponent<Rigidbody>().mass = 0.6f;
							block.GetComponent<HingeJoint>().breakForce = 30000.0f;
							block.GetComponent<HingeJoint>().breakTorque = 30000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Wing":
							block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Ballast":
							block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "MetalJaw":
							BB.Prefab.myDamageType = DamageType.Blunt;
							block.GetComponent<ConfigurableJoint>().breakForce = 50000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 50000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SteeringBlock":
							block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SteeringHinge":
							block.GetComponent<Rigidbody>().mass = 0.4f;
							block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Hinge":
							block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "BallJoint":
							block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Decoupler":
							block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Flamethrower":
							block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "CircularSaw":
							BB.Prefab.myDamageType = DamageType.Blunt;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "MetalBall":
							BB.Prefab.myDamageType = DamageType.Blunt;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Propeller":
							BB.Prefab.myDamageType = DamageType.Blunt;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SmallPropeller":
							BB.Prefab.myDamageType = DamageType.Blunt;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SingleWoodenBlock":
							configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
							foreach (ConfigurableJoint joint in configurablejoints)
							{
								joint.breakForce = 55000.0f;
								joint.breakTorque = 55000.0f;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "DoubleWoodenBlock":
							configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
							foreach (ConfigurableJoint joint in configurablejoints)
							{
								joint.breakForce = 50000.0f;
								joint.breakTorque = 50000.0f;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "WoodenPole":
							configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
							foreach (ConfigurableJoint joint in configurablejoints)
							{
								joint.breakForce = 40000.0f;
								joint.breakTorque = 40000.0f;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "ArmorPlateSmall":
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "ArmorPlateLarge":
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							block.GetComponent<Rigidbody>().mass = 0.5f;
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "ArmorPlateRound":
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "SpinningBlock":
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.05f;
								collider.material.dynamicFriction = 0.05f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "Grabber":
							block.GetComponent<Rigidbody>().mass = 0.7f;
							block.GetComponent<ConfigurableJoint>().breakForce = 13750.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 13750.0f;
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.01f;
								collider.material.dynamicFriction = 0.01f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
						case "WoodenPanel":
							block.GetComponent<Rigidbody>().mass = 0.5f;
							block.GetComponent<ConfigurableJoint>().breakForce = Mathf.Infinity;
							block.GetComponent<ConfigurableJoint>().breakTorque = Mathf.Infinity;
							colliders = block.GetComponentsInChildren<BoxCollider>();
							foreach (BoxCollider collider in colliders)
							{
								collider.material.staticFriction = 0.1f;
								collider.material.dynamicFriction = 0.1f;
								collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
							}
							//Console.Log("Modified properties of: " + block.name);
							break;
					}
				}

				//Adding Components
				
				if (frictionblocks.Contains(block.name))
				{
					if (block.GetComponent<FrictionController>() == null) { block.AddComponent<FrictionController>(); }
				}
				if (invtoggleblocks.Contains(block.name))
				{
					if (block.GetComponent<InvincibilityToggler>() == null) { block.AddComponent<InvincibilityToggler>(); }
				}
				if (invremoveblocks.Contains(block.name))
				{
					if (block.GetComponent<InvincibilityRemover>() == null) { block.AddComponent<InvincibilityRemover>(); }
				}
				if (dragfixblocks.Contains(block.name))
				{
					if (block.GetComponent<DragFix>() == null) { block.AddComponent<DragFix>(); }
				}
				if (roundwheelblocks.Contains(block.name))
				{
					if (block.GetComponent<RoundWheels>() == null) { block.AddComponent<RoundWheels>(); }
				}
				if (smokefixblocks.Contains(block.name))
				{
					if (block.GetComponent<SmokeFix>() == null) { block.AddComponent<SmokeFix>(); }
				}
				if (spinuptimeblocks.Contains(block.name))
				{
					if (block.GetComponent<SpinupTime>() == null) { block.AddComponent<SpinupTime>(); }
				}
				if (block.name == "Suspension")
				{
					if (block.GetComponent<Pneumatics>() == null) { block.AddComponent<Pneumatics>(); }
				}
				if (block.name == "Spring" || block.name == "RopeWinch")
				{
					if (block.GetComponent<WinchFix>() == null) { block.AddComponent<WinchFix>(); }
				}
				if (sfxblocks.Contains(block.name))
				{
					if (block.GetComponent<ImpactSparksFix>() == null) { block.AddComponent<ImpactSparksFix>(); }
				}
				if (block.name == "Sensor")
				{
					if (block.GetComponent<SensorDisjoint>() == null) { block.AddComponent<SensorDisjoint>(); }
				}
				if (block.name == "Grabber")
				{
					if (block.GetComponent<ExplosionStopper>() == null) { block.AddComponent<ExplosionStopper>(); }
					if (block.GetComponent<GrabberModifier>() == null) { block.AddComponent<GrabberModifier>(); }
				}

			};
			//Multiverse Cannonball tick damage removal 
			Modding.Events.OnConnect += delegate ()
			{
				try
				{
					Component[] CannonBallTickDamagers = UnityEngine.GameObject.Find("PROJECTILES").GetComponentsInChildren<CannonBallDamage>(true);
					foreach (Component CBT in CannonBallTickDamagers)
					{
						UnityEngine.Object.Destroy(CBT);
					}
				}
				catch (NullReferenceException)
				{
					ModConsole.Log("Could not find GameObject PROJECTILES when logging into a multiverse session.");
				}
			};
			//PrefabModder.ModPrefab();
			OptionsMaster.defaultSmoothness = 0f;
			Physics.gravity = new Vector3(Physics.gravity.x, -55f, Physics.gravity.z);
			OptionsMaster.BesiegeConfig.MorePrecisePhysics = false;
			StatMaster.Rules.DisableFire = true;
			
			Console.Log("Loaded TE-BBMod Version 1.0.6");
		}

		public void SetupNetworking()
		{
			Pneumatics.SetupNetworking();
			SmokeFix.SetupNetworking();
			WinchFix.SetupNetworking();

			ModNetworking.CallbacksWrapper callbacksWrapper = ModNetworking.Callbacks;

			Messages.emitsmallsparks = ModNetworking.CreateMessageType(new DataType[]
			{
					DataType.Vector3,
					DataType.Vector3,
					DataType.Block
			});
			MessageType messageType = Messages.emitsmallsparks;
			callbacksWrapper[messageType] += new Action<Message>(ImpactSparksFix.ProcessSmallHit);

			Messages.emitbigsparks = ModNetworking.CreateMessageType(new DataType[]
			{
					DataType.Vector3,
					DataType.Vector3,
					DataType.Block
			});
			messageType = Messages.emitbigsparks;
			callbacksWrapper[messageType] += new Action<Message>(ImpactSparksFix.ProcessHugeHit);

			//Debug.Log("Setup Networking OK");
		}
	}
}
