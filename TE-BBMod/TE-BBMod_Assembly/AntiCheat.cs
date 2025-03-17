using Modding.Blocks;
using UnityEngine;
using Console = Modding.ModConsole;
namespace MysticFix
{
    class AntiCheat : MonoBehaviour
    {
        private GameObject block;
        private BlockBehaviour BB;
        private int tickCount;
        private float speed;
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
            //Check Scaling
            if (block.transform.localScale != Vector3.one)
            {
                Console.Log(block.name +" is scaled by "+ block.transform.localScale);
            }
        }
        void FixedUpdate()
        {
            if (tickCount < 5)
            {
                tickCount++;
            }
            else
            {
                switch(block.name)
                {
                case "SteeringBlock": //SteeringWheel
                case "SteeringHinge":
                    speed = BB.GetComponent<SteeringWheel>().SpeedSlider.Value;
                    if(speed < 0f || speed > 3.0f) Console.Log(block.name + " is set to " + speed + " speed");
                    break;
                case "CircularSaw": //CogMotorControllerHinge
                case "LargeWheel":
                case "CogMediumPowered":
                case "Wheel":
                case "Drill":
                case "SpinningBlock":
                speed = BB.GetComponent<CogMotorControllerHinge>().SpeedSlider.Value;
                    if(speed < 0f || speed > 2.0f) Console.Log(block.name + " is set to " + speed + " speed");
                    break;
                case "FlyWheel": //FlyWheelBlock
                speed = BB.GetComponent<FlyWheelBlock>().InertiaSlider.Value;
                    if(speed < 0f || speed > 10.0f) Console.Log(block.name + " is set to " + speed + " Inertia");
                    break;
                case "Suspension": //SuspensionController
                speed = BB.GetComponent<SuspensionController>().SpringSlider.Value;
                    if(speed < 0f || speed > 3.0f) Console.Log(block.name + " is set to " + speed + " spring");
                speed = BB.GetComponent<SuspensionController>().DamperSlider.Value;
                    if(speed < 0f || speed > 3.0f) Console.Log(block.name + " is set to " + speed + " damper");
                    break;
                case "Piston": //SliderCompress
                speed = BB.GetComponent<SliderCompress>().SpeedSlider.Value;
                    if(speed < 0f || speed > 2.0f) Console.Log(block.name + " is set to " + speed + " speed");
                speed = BB.GetComponent<SliderCompress>().StrengthSlider.Value;
                    if(speed < 0f || speed > 2.0f) Console.Log(block.name + " is set to " + speed + " power");
                    break;
                case "Spring": //SpringCode
                case "RopeWinch": 
                speed = BB.GetComponent<SpringCode>().SpeedSlider.Value;
                    if(speed < 0f || speed > 10.0f) Console.Log(block.name + " is set to " + speed + " power");
                    break;
                case "MetalJaw": //SpringReleaseBlock
                speed = BB.GetComponent<SpringReleaseBlock>().ForceSlider.Value;
                    if(speed < 0f || speed > 2.0f) Console.Log(block.name + " is set to " + speed + " force");
                speed = BB.GetComponent<SpringReleaseBlock>().AngleSlider.Value;
                    if(speed < 0f || speed > 90.0f) Console.Log(block.name + " is set to " + speed + " degrees");
                    break;
                case "Cannon": //CanonBlock 
                case "ShrapnelCannon":
                speed = BB.GetComponent<CanonBlock>().StrengthSlider.Value;
                    if(speed < 0f || speed > 4.0f) Console.Log(block.name + " is set to " + speed + " power");
                    break;
                case "Vacuum": //VacuumBlock
                speed = BB.GetComponent<VacuumBlock>().PowerSlider.Value;
                    if(speed < 0f || speed > 4.0f) Console.Log(block.name + " is set to " + speed + " power");
                    break;
                case "WaterCannon": //WaterCannonController
                speed = BB.GetComponent<WaterCannonController>().StrengthSlider.Value;
                    if(speed < 0f || speed > 4.0f) Console.Log(block.name + " is set to " + speed + " power");
                    break;
                case "FlyingBlock": //FlyingController
                speed = BB.GetComponent<FlyingController>().SpeedSlider.Value;
                    if(speed < 0f || speed > 2.0f) Console.Log(block.name + " is set to " + speed + " speed");
                    break;
                case "Ballast": //BallastWeightController    
                speed = BB.GetComponent<BallastWeightController>().MassSlider.Value;
                    if(speed < 0f || speed > 3.0f) Console.Log(block.name + " is set to " + speed + " mass");
                    break;
                case "DragBlock": //DragBlock
                speed = BB.GetComponent<DragBlock>().MagSlider.Value;
                    if(speed < 0f || speed > 3.0f) Console.Log(block.name + " is set to " + speed + " magnitude");
                    break;
                case "Balloon": //BalloonController
                speed = BB.GetComponent<BalloonController>().BuoyancySlider.Value;
                    if(speed < 0f || speed > 1.5f) Console.Log(block.name + " is set to " + speed + " lift");
                speed = BB.GetComponent<BalloonController>().StringLengthSlider.Value;
                    if(speed < 0f || speed > 6.0f) Console.Log(block.name + " is set to " + speed + " string length");
                    break;
                case "SqrBalloon": //SqrBalloonController
                speed = BB.GetComponent<SqrBalloonController>().PowerSlider.Value;
                    if(speed < 0f || speed > 1.5f) Console.Log(block.name + " is set to " + speed + " lift");
                    break;
                case "Sensor": //SensorBlock
                speed = BB.GetComponent<SensorBlock>().DistanceSlider.Value;
                    if(speed < 0f || speed > 50f) Console.Log(block.name + " is set to " + speed + " distance");
                speed = BB.GetComponent<SensorBlock>().RadiusSllider.Value;
                    if(speed < 0f || speed > 2f) Console.Log(block.name + " is set to " + speed + " radius");
                speed = BB.GetComponent<SensorDisjoint>().DS.Value;
                    if(speed < 0f || speed > 4f) Console.Log(block.name + " is set to " + speed + " disjoint");
                    break;
                }
            Destroy(this);
            }
        }
    }
}