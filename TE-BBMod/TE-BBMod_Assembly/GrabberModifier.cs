using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    public class GrabberModifier : MonoBehaviour
    {
        private BlockBehaviour BB;
        private int fcounter;
        private bool firstframe = false;
        private MMenu GrabberMenu;
        private int GrabberSelector = 0;
        public float force = 13750.0f;
        private ConfigurableJoint[] CJ;
        public int selectedmode;

        internal static List<string> grabberModes = new List<string>()
        {
            "Vanilla",
            "MGrabber",
            "UGrabber",
            "DGrabber"
        };

        private void Awake()
        {
            Console.Log("Grabber Modifier Added to "+gameObject.name);
            BB = GetComponentInParent<BlockBehaviour>();
            if (this.BB == null)
			{
                Console.Log("No block behavior, destroying the object");
				UnityEngine.Object.Destroy(this);
			}
            GrabberMenu = BB.AddMenu("Mode", GrabberSelector, grabberModes, false);
            GrabberMenu.ValueChanged += (ValueHandler)(value => 
            {
                selectedmode=value;
                switch (value)
                {
                    case 0:
                        force = 13750.0f;
                        break;
                    case 1:
                        force = 100000.0f;
                        break;
                    case 2:
                        force = 150000.0f;
                        break;
                    case 3:
                        force = 200000.0f;
                        break;
                } 
            });
        }

        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (selectedmode == 0)
                        return;
                    if (!firstframe)
                    {
                        fcounter++;
                        CJ = GetComponents<ConfigurableJoint>();
                        foreach (ConfigurableJoint joint in CJ)
                        {
                            joint.breakForce = force;
                            joint.breakTorque = force;
                        }
                        if (fcounter == 10)
                            firstframe = true;
                    }
                }
            }
        }
    }
}