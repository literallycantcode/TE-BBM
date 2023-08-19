using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{
    public class InvincibilityToggler : MonoBehaviour
    {

        private BlockBehaviour BB;
        private bool MakeInvinc = false;
        private MToggle MI;
        private ConfigurableJoint CJ;
        private bool firstframe = false;
        private int fcounter;

        private void Awake()
        {
            Console.Log("Invincibility Toggler Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            MI = BB.AddToggle("Make Invincible", "MVI", MakeInvinc);
            MI.Toggled += (bool value) => { MakeInvinc = value; };

            MI.DisplayInMapper = true;

            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (!GetComponent<ConfigurableJoint>())
                    return;
                CJ = GetComponent<ConfigurableJoint>();
            }
        }
        void Update()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (!firstframe)
                    {
                        if (!CJ)
                        {
                            firstframe = true;
                            return;
                        }
                        fcounter++;

                        if (MakeInvinc)
                            {
                            CJ.breakForce = Mathf.Infinity;
                            CJ.breakTorque = Mathf.Infinity;
                            }
                        else
                        {
                            return;
                        }
                       
                        if (fcounter == 4)
                            firstframe = true;
                    }
                }
            }
        }
    }

}