using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{
    public class FrictionController : MonoBehaviour
    {

        private Collider[] colliders;
        private BlockBehaviour BB;
        private int fcounter;
        private MSlider GS;
        private MMenu PCMenu;
        private int PCselect = 0;
        public float grip = 0.7f;
        private bool firstframe = false;
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
            //Console.Log("Friction Controller Added to "+gameObject.name);
            BB = GetComponentInParent<BlockBehaviour>();
            if (this.BB == null)
            {
                Console.Log("No block behavior, destroying the object");
                UnityEngine.Object.Destroy(this);
            }
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

            MSlider frictionSlider = BB.AddSlider("Friction", "Friction", grip, 0.1f, 4.0f, "", "x");
            frictionSlider.ValueChanged += (float value) =>
            {
                grip = value;
            };

            frictionSlider.DisplayInMapper = true;
            PCMenu.DisplayInMapper = true;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                colliders = GetComponentsInChildren<Collider>();
                foreach (Collider collider in colliders)
                {
                    collider.material.dynamicFriction = grip;
                    collider.material.staticFriction = grip;
                    collider.material.frictionCombine = PC;
                }
            }
        }
        private void FixedUpdate()
        {
            if (!firstframe)
            {
                if (!StatMaster.isClient || StatMaster.isLocalSim)
                {
                    if (BB.isSimulating)
                    {


                        if (fcounter > 5)
                        { fcounter++; }
                        else
                            //modifiying collider at runtime too because roundwheel colliders are generated at runtime
                            colliders = GetComponentsInChildren<Collider>();
                        foreach (Collider collider in colliders)
                        {
                            collider.material.dynamicFriction = grip;
                            collider.material.staticFriction = grip;
                            collider.material.frictionCombine = PC;
                        }
                        if (grip < 0.1f || grip > 4) Console.Log(gameObject.name + " is set to " + grip + " friction");
                        firstframe = true;
                    }
                }
            }
        }
    }
}