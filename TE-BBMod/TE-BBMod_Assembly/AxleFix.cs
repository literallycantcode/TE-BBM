using Modding.Blocks;
using UnityEngine;

namespace MysticFix
{

    class AxleFix : MonoBehaviour
    {
    public BlockBehaviour BB;
    public Component[] configurablejoints;
    public Component[] hingejoints;
    private bool firstframe = false;
    private int fcounter;

        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
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
                        configurablejoints=BB.GetComponentsInChildren<ConfigurableJoint>();
                        hingejoints=BB.GetComponentsInChildren<HingeJoint>();
                        foreach( ConfigurableJoint cJoint in configurablejoints )
                        {
                            cJoint.breakForce=60000.0f;
                            cJoint.breakTorque=60000.0f;
                        }
                        foreach( HingeJoint hJoint in hingejoints )
                        {
                            hJoint.breakForce=60000.0f;
                            hJoint.breakTorque=60000.0f;
                        }
                       
                        if (fcounter == 4)
                            firstframe = true;
                    }
                }
            }
        }
    }
}
