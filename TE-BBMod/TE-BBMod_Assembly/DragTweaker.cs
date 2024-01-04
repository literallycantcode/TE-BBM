using UnityEngine;
using Modding;
using Console = Modding.ModConsole;
namespace MysticFix
{
    class DragTweaker : MonoBehaviour
    {
        private GameObject block;
        private int tickCount;
        void Awake()
        {
            tickCount = 0;
            if (gameObject != null)
            {
                block = gameObject;
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
                tickCount = tickCount + 1;
            }
            else
            {
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
                            Console.Log("Modified drag of: " + block.name);
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
                            Console.Log("Modified drag of: " + block.name);
                            break;

                        case "Grabber":
                            block.GetComponent<Rigidbody>().angularDrag = 0;
                            block.GetComponent<Rigidbody>().drag = 0.01f;
                            block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                            Console.Log("Modified drag of: " + block.name);
                            break;
                    }
                }
                Destroy(this);
            }
        }
    }
}

