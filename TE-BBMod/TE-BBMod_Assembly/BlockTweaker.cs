using UnityEngine;
using Modding;
using Console = Modding.ModConsole;
namespace MysticFix
{
    class BlockTweaker : MonoBehaviour
    {
        private GameObject block;
        private int tickCount;
        void Awake()
        {
            block = gameObject;
            tickCount = 0;
        }
        void FixedUpdate()
        {
            Component[] configurablejoints;
            Component[] colliders;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (block != null)
                {
                    if (tickCount < 10)
                    {
                        tickCount = tickCount + 1;
                    }
                    else
                    {
                        switch (block.name)
                        {
                            case "Drill":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "WheelUnpowered":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "ShrapnelCannon":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Torch":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "GripPad":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Log":
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "MetalBlade":
                                block.GetComponent<Rigidbody>().mass = 0.6f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<ConfigurableJoint>().breakForce = 80000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 80000.0f;
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Spike":
                                block.GetComponent<Rigidbody>().mass = 0.6f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<ConfigurableJoint>().breakForce = 80000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 80000.0f;
                                colliders = block.GetComponentsInChildren<CapsuleCollider>();
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Wheel":
                                block.GetComponent<HingeJoint>().breakForce = 60000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Suspension":
                                block.GetComponent<ConfigurableJoint>().breakForce = 35000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 35000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Piston":
                                block.GetComponent<ConfigurableJoint>().breakForce = 35000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 35000.0f;

                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SmallWheel":
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<HingeJoint>().breakForce = 60000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "LargeWheel":
                                block.GetComponent<HingeJoint>().breakForce = 70000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 70000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "CogMediumPowered":
                                block.GetComponent<HingeJoint>().breakForce = 90000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 90000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "CogMediumUnpowered":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "CogLargeUnpowered":
                                block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "LargeWheelUnpowered":
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 500;
                                block.GetComponent<HingeJoint>().breakForce = 60000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 60000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "FlyingBlock":
                                block.GetComponent<ConfigurableJoint>().breakForce = 20000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 20000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Axle":
                                configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
                                foreach (ConfigurableJoint joint in configurablejoints)
                                {
                                    joint.breakForce = 60000.0f;
                                    joint.breakTorque = 60000.0f;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "WingPanel":
                                block.GetComponent<ConfigurableJoint>().breakForce = 40000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 40000.0f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Swivel":
                                block.GetComponent<Rigidbody>().mass = 0.6f;
                                block.GetComponent<HingeJoint>().breakForce = 30000.0f;
                                block.GetComponent<HingeJoint>().breakTorque = 30000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Wing":
                                block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Ballast":
                                block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "MetalJaw":
                                block.GetComponent<ConfigurableJoint>().breakForce = 50000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 50000.0f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SteeringBlock":
                                block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SteeringHinge":
                                block.GetComponent<Rigidbody>().mass = 0.4f;
                                block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Hinge":
                                block.GetComponent<ConfigurableJoint>().breakForce = 50000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 50000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "BallJoint":
                                block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Decoupler":
                                block.GetComponent<ConfigurableJoint>().breakForce = 30000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 30000.0f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Flamethrower":
                                block.GetComponent<ConfigurableJoint>().breakForce = 60000.0f;
                                block.GetComponent<ConfigurableJoint>().breakTorque = 60000.0f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "CircularSaw":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "MetalBall":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Propeller":
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SmallPropeller":
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SingleWoodenBlock":
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
                                foreach (ConfigurableJoint joint in configurablejoints)
                                {
                                    joint.breakForce = 55000.0f;
                                    joint.breakTorque = 55000.0f;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "DoubleWoodenBlock":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
                                foreach (ConfigurableJoint joint in configurablejoints)
                                {
                                    joint.breakForce = 50000.0f;
                                    joint.breakTorque = 50000.0f;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "WoodenPole":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                configurablejoints = block.GetComponentsInChildren<ConfigurableJoint>();
                                foreach (ConfigurableJoint joint in configurablejoints)
                                {
                                    joint.breakForce = 40000.0f;
                                    joint.breakTorque = 40000.0f;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "StartingBlock":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Plow":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "HalfPipe":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "ArmorPlateSmall":
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "ArmorPlateLarge":
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<Rigidbody>().mass = 0.5f;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "ArmorPlateRound":
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Cannon":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "WaterCannon":
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "SpinningBlock":
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.05f;
                                    collider.material.dynamicFriction = 0.05f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "Grabber":
                                block.GetComponent<Rigidbody>().mass = 0.7f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0.01f;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.01f;
                                    collider.material.dynamicFriction = 0.01f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                            case "WoodenPanel":
                                block.GetComponent<Rigidbody>().mass = 0.5f;
                                block.GetComponent<Rigidbody>().angularDrag = 0;
                                block.GetComponent<Rigidbody>().drag = 0;
                                block.GetComponent<Rigidbody>().maxAngularVelocity = 100;
                                block.GetComponent<ConfigurableJoint>().breakForce = Mathf.Infinity;
                                block.GetComponent<ConfigurableJoint>().breakTorque = Mathf.Infinity;
                                colliders = block.GetComponentsInChildren<BoxCollider>();
                                foreach (BoxCollider collider in colliders)
                                {
                                    collider.material.staticFriction = 0.1f;
                                    collider.material.dynamicFriction = 0.1f;
                                    collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
                                }
                                Console.Log("Modified properties of: " + block.name);
                                break;
                        }
                        Destroy(this);
                    }
                }
                else
                {
                    Console.Log("No Gameobject");
                    Destroy(this);
                }
            }
            else
            {
                Destroy(this);
            }
        }
    }
}
