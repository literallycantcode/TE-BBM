using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{

    public class ExplosionStopper : MonoBehaviour
    {
        private BlockBehaviour BB;
        private int fcounter;
        private bool firstframe = false;

        private void Awake()
        {
            Console.Log("Explosion Stopper Added to "+gameObject.name);
            BB = GetComponent<BlockBehaviour>();
            if (this.BB == null)
			{
                Console.Log("No block behavior, destroying the object");
				UnityEngine.Object.Destroy(this);
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
                        fcounter++;
                        JoinOnTriggerBlock componentInChildren = base.gameObject.GetComponentInChildren<JoinOnTriggerBlock>();
                        if (componentInChildren != null && componentInChildren.isJoined)
                        {
                            componentInChildren.currentJoint.projectionMode = 1;
                            componentInChildren.currentJoint.projectionDistance = 1.25f;
                            componentInChildren.currentJoint.projectionAngle = 100f;
                        }
                        if (fcounter == 10)
                            firstframe = true;
                    }
                }
            }
        }
    }
}