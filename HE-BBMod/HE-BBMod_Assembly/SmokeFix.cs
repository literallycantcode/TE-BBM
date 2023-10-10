using Modding;
using Modding.Blocks;
using UnityEngine;

namespace MysticFix
{
    [RequireComponent(typeof(BlockBehaviour))]
    [RequireComponent(typeof(SoundOnCollide))]
    
    class SmokeFix : FrameDelayAction
    {
        private BlockBehaviour BB;
        private SoundOnCollide SOC;
        private MToggle SmokeToggle;
        private static MessageType mToggleDust;
        private bool dustActive = true;
        protected override int FRAMECOUNT { get; } = 1;
        protected override bool DESTROY_AT_END { get; } = false;
        internal static void SetupNetworking()
        {
            mToggleDust = ModNetworking.CreateMessageType(DataType.Block, DataType.Boolean);
            ModNetworking.Callbacks[mToggleDust] += (System.Action<Message>)delegate (Message m)
            {
                Block target = (Block)m.GetData(0);
                if (target == null) return;
                else target.InternalObject.GetComponent<SmokeFix>().dustActive = (bool)m.GetData(1);
            };
        }
        new void Awake()
        {
            base.Awake();
            BB = GetComponent<BlockBehaviour>();
            SOC = GetComponent<SoundOnCollide>();

            SmokeToggle = BB.AddToggle("Enable Smoke", "Enable Smoke", dustActive);
            SmokeToggle.Toggled += (bool value) =>
            {
                dustActive = value;
                if (!BB.SimPhysics) return;
                ModNetworking.SendToAll(mToggleDust.CreateMessage(BB, dustActive));
            };
        }

        protected override void DelayedAction()
        {
            if (!BB.SimPhysics || SOC == null) return;

            if (!dustActive) SOC.cutoff = float.MaxValue;
        }
    }
}