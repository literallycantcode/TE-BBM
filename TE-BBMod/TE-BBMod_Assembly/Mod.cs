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

				//Adding Components
				if (block.GetComponent<BlockTweaker>() == null) { block.AddComponent<BlockTweaker>(); }
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
				if (block.name == "Axle")
				{
					if (block.GetComponent<AxleFix>() == null) { block.AddComponent<AxleFix>(); }
				}
				if (block.name == "Grabber")
				{
					if (block.GetComponent<ExplosionStopper>() == null) { block.AddComponent<ExplosionStopper>(); }
					if (block.GetComponent<GrabberModifier>() == null) { block.AddComponent<GrabberModifier>(); }
				}

				BB.Prefab.myDamageType = DamageType.Blunt;

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
			
			Debug.Log("Loaded TE-BBMod Version 1.0.3");
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
