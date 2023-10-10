using Modding.Blocks;
using Modding.Common;
using UnityEngine;

namespace MysticFix
{
    [RequireComponent(typeof(BlockBehaviour))]
    abstract class FrameDelayAction : MonoBehaviour
    {
        //The number of frames to wait before invoking delayedAction
        protected virtual int FRAMECOUNT { get; } = -1;
        protected virtual bool DESTROY_AT_END { get; } = true;

        //This block in Block format
        protected Block thisBlock;

        protected void Awake()
        {
            thisBlock = Block.From(gameObject);

            if (thisBlock.InternalObject.SimPhysics)
            {
                //This Enumerator is only meant to be executed on the machines that are being simulated by this computer.
                if (Player.GetHost() == null || (thisBlock.Machine.Player == Player.GetLocalPlayer() ? Player.GetLocalPlayer().IsHost || Player.GetLocalPlayer().InLocalSim : Player.GetLocalPlayer().IsHost && !thisBlock.Machine.Player.InLocalSim)) StartCoroutine(DelayedActionEnumerator());
                else Destroy(this);
            }
        }

        private System.Collections.IEnumerator DelayedActionEnumerator()
        {
            if (FRAMECOUNT == -1) throw new System.InvalidOperationException("FRAMECOUNT not overriden");
            //Wait FRAMECOUNT FixedUpdates into sim
            for (int i = 0; i < FRAMECOUNT; i++) yield return new WaitForFixedUpdate();

            //Take action
            DelayedAction();

            //This component instance is destroyed after the necessary changes are made, if the option is enabled. (it is by default)
            if(DESTROY_AT_END) Destroy(this);
        }

        protected abstract void DelayedAction();
    }
}
