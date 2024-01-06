using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    public class InvincibilityRemover : MonoBehaviour
    {
        private BlockBehaviour BB;
        private bool MakeInvinc = false;
        private MToggle MI;
        private HingeJoint HJ;
        private bool firstframe = false;
        private int fcounter;

        private void Awake()
        {
            Console.Log("Invincibility Remover Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            if (this.BB == null)
			{
                Console.Log("No block behavior, destroying the object");
				UnityEngine.Object.Destroy(this);
			}
            if(gameObject.name=="CircularSaw" || gameObject.name=="CogMediumPowered")
            {
                MakeInvinc = false;
            }
            else
            { 
                MI = BB.AddToggle("Make Invincible", "MVI", MakeInvinc);
                MI.Toggled += (bool value) => { MakeInvinc = value; };

                MI.DisplayInMapper = true;
            }
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (!GetComponent<HingeJoint>())
                    return;
                HJ = GetComponent<HingeJoint>();
            }
        }

        void FixedUpdate()
        {
            if (!StatMaster.isClient || StatMaster.isLocalSim)
            {
                if (BB.SimPhysics)
                {
                    if (!firstframe)
                    {
                        if (!HJ)
                        {
                            firstframe = true;
                            return;
                        }
                        fcounter++;

                        if (MakeInvinc)
                        {
                            HJ.breakForce = Mathf.Infinity;
                            HJ.breakTorque = Mathf.Infinity;
                        }
                        else
                        {
                            if(gameObject.name=="CogMediumPowered")
                            {
                                HJ.breakForce = 90000;
                                HJ.breakTorque = 90000;
                            }
                            else
                            {
                                HJ.breakForce = 100000;
                                HJ.breakTorque = 100000;
                            }

                        }
                       
                        if (fcounter == 5)
                            firstframe = true;
                    }
                }
            }
        }
    }
}