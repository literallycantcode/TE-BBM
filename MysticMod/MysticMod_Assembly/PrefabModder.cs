using MysticFix;
using UnityEngine;
using Modding.Blocks;
using System.Collections.Generic;

namespace MysticFix
{

    public class PrefabModder : MonoBehaviour
    {

        internal static void ModPrefab()
        {
            BlockBehaviour prefab;
			PrefabMaster.GetBlock(BlockType.SteeringBlock, out prefab);
			//((SteeringWheel)prefab).allowLimits = true;
            Modding.Events.OnBlockInit += delegate (Block toInit)
            {
                if (Modding.Game.IsSimulating) return;
                prefab = toInit.InternalObject;
                ApplyComponents(prefab);
            };
        }
        
        private static void ApplyComponents(BlockBehaviour BB)
        {
                if (BB.Prefab.Type==BlockType.SteeringBlock)
                {
                    ((SteeringWheel)BB).allowLimits = true;
                    BB.SendMessage("Awake", SendMessageOptions.DontRequireReceiver);
                    //BB.gameObject.AddComponent<SteeringBlockLimiter>();
                }
        }
    }
}