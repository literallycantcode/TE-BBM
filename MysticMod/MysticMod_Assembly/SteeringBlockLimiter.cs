using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    [RequireComponent(typeof(BlockBehaviour))]
    public class SteeringBlockLimiter : MonoBehaviour
    {
        private void Start()
        {
            Console.Log("Steering Block Limiter Added to "+gameObject.name);
            BlockBehaviour BB = base.GetComponent<BlockBehaviour>();
            if(BB is SteeringWheel)
            {
                Console.Log("Attempting to restore steering block limits");
                ((SteeringWheel)BB).allowLimits = true;
                BB.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
                if(gameObject.name=="SteeringBlock" && !Modding.Game.IsSimulating && ((SteeringWheel)BB).LimitsSlider != null)
                {
                    Console.Log("Tries to modify limits slider");
                    //(BB as SteeringWheel).LimitsSlider.UseLimitsToggle.IsActive = false;
                    //(BB as SteeringWheel).LimitsSlider.UseLimitsToggle.ApplyValue();
                    (BB as SteeringWheel).LimitsSlider.iconInfo = new FauxTransform(new Vector3(0f,0f,0f), Quaternion.Euler(0f,0f,0f),Vector3.one*0.35f);
                }
            }
        }
    }

}