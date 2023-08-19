using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    public class SteeringBlockLimiter : MonoBehaviour
    {
        private BlockBehaviour BB;
        private void Start()
        {
            Console.Log("Steering Block Limiter Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            if(BB is SteeringWheel)
            {
                ((SteeringWheel)BB).allowLimits = true;
                FauxTransform fauxTransform = new FauxTransform(new Vector3(0f,0f,0f), Quaternion.Euler(0f,0f,0f),Vector3.one*0.35f)
                (BB as SteeringWheel).LimitsSlider.UseLimitsToggle.IsActive = false;
                (BB as SteeringWheel).LimitsSlider.UseLimitsToggle.ApplyValue();
                (BB as SteeringWheel).LimitsSlider.iconInfo = fauxTransform;
            }
        }
    }

}