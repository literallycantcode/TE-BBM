using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;

namespace MysticFix
{
    public class FrictionController : SimBehaviour
    {

        private BlockBehaviour BB;
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 0;
        public PhysicMaterialCombine PC = PhysicMaterialCombine.Average;

        internal static List<string> PCmenul = new List<string>()
        {
            "Average",
            "Multiply",
            "Minimum",
            "Maximum",
        };
        

        private void Awake()
        {
            BB = GetComponentInParent<BlockBehaviour>();
            PCMenu = BB.AddMenu("Combine", PCselect, PCmenul, false);
            PCMenu.ValueChanged += (ValueHandler)(value => 
            {
                switch (value)
                {
                    case 0:
                        PC = PhysicMaterialCombine.Average;
                        break;
                    case 1:
                        PC = PhysicMaterialCombine.Multiply;
                        break;
                    case 2:
                        PC = PhysicMaterialCombine.Minimum;
                        break;
                    case 3:
                        PC = PhysicMaterialCombine.Maximum;
                        break;
                }
            });
            
            MSlider frictionSlider = BB.AddSlider("Friction","Friction",1.0f,0.1f,4.0f);
            
            frictionSlider.ValueChanged += (float value) => {
                Component[] colliders = colliders=GetComponentsInChildren<BoxCollider>();
						foreach(BoxCollider collider in colliders)
						{
							collider.material.staticFriction=value;
							collider.material.dynamicFriction=value;
							collider.material.frictionCombine=PC;
						}
                };
        }
        private void FixedUpdate()
        {

        }
    }
}