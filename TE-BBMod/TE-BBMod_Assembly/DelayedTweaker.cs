using UnityEngine;
using Modding;
using Console = Modding.ModConsole;
namespace MysticFix
{
    class DelayedTweaker : MonoBehaviour
    {
        private GameObject block;
        private BlockBehaviour BB;
        private int tickCount;
        void Awake()
        {
            tickCount = 0;
            if (gameObject != null)
            {
                block = gameObject;
                BB = GetComponent<BlockBehaviour>();
            }
            else
            {
                Console.Log("No Gameobject");
                Destroy(this);
            }
        }
        void FixedUpdate()
        {
            if (tickCount < 10)
            {
                tickCount++;
            }
            else
            {
                //Tweak Drags
                if (block.GetComponent<Rigidbody>() != null)
                {
                    switch (block.name)
                    {
                        case "Drill":
                        case "FlyingBlock":
                        case "CircularSaw":
                        case "SpinningBlock":
                            block.GetComponent<Rigidbody>().angularDrag = 0;
                            block.GetComponent<Rigidbody>().drag = 0;
                            //Console.Log("Modified drag of: " + block.name);
                            break;

                        case "WheelUnpowered":
                        case "ShrapnelCannon":
                        case "Torch":
                        case "GripPad":
                        case "Log":
                        case "MetalBlade":
                        case "Spike":
                        case "Wheel":
                        case "Suspension":
                        case "SmallWheel":
                        case "LargeWheel":
                        case "CogMediumPowered":
                        case "CogMediumUnpowered":
                        case "CogLargeUnpowered":
                        case "LargeWheelUnpowered":
                        case "Axle":
                        case "Swivel":
                        case "Ballast":
                        case "MetalJaw":
                        case "SteeringBlock":
                        case "SteeringHinge":
                        case "Hinge":
                        case "BallJoint":
                        case "Decoupler":
                        case "Flamethrower":
                        case "MetalBall":
                        case "SingleWoodenBlock":
                        case "DoubleWoodenBlock":
                        case "WoodenPole":
                        case "StartingBlock":
                        case "Plow":
                        case "HalfPipe":
                        case "ArmorPlateSmall":
                        case "ArmorPlateLarge":
                        case "ArmorPlateRound":
                        case "Cannon":
                        case "WaterCannon":
                        case "WoodenPanel":
                            block.GetComponent<Rigidbody>().maxAngularVelocity = 500;
                            block.GetComponent<Rigidbody>().angularDrag = 0;
                            block.GetComponent<Rigidbody>().drag = 0;
                            //Console.Log("Modified drag of: " + block.name);
                            break;

                        case "Grabber":
                            block.GetComponent<Rigidbody>().angularDrag = 0;
                            block.GetComponent<Rigidbody>().drag = 0.01f;
                            block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                            //Console.Log("Modified drag of: " + block.name);
                            break;
                    }
                    //Other Tweaks
                    switch (block.name)
                    {
                        case "Hinge":
                            block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
							block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
                            break;
                        case "Axle":
                            Component[] configurablejoints = BB.GetComponentsInChildren<ConfigurableJoint>();
                            Component[] hingejoints = BB.GetComponentsInChildren<HingeJoint>();
                            foreach( ConfigurableJoint cJoint in configurablejoints )
                            {
                                cJoint.breakForce=60000.0f;
                                cJoint.breakTorque=60000.0f;
                            }
                            foreach( HingeJoint hJoint in hingejoints )
                            {
                                hJoint.breakForce=60000.0f;
                                hJoint.breakTorque=60000.0f;
                            }
                            break;   
                    }
                }
                Destroy(this);
            }
        }
    }
}

