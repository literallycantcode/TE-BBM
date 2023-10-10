using Modding.Blocks;
using UnityEngine;

namespace MysticFix
{
    class DragFix : MonoBehaviour
    {
        Block thisBlock;
        private void Awake()
        {
            thisBlock = Block.From(gameObject);
            AxialDrag AD = thisBlock.InternalObject.GetComponent<AxialDrag>();
            float VC = AD.velocityCap;

            //Mapper definition
            MToggle ADtoggle = thisBlock.InternalObject.AddToggle("Disable Drag", "Disable drag", false);
            ADtoggle.Toggled += (bool value) => { AD.velocityCap = value ? 0 : VC; };
        }
    }
}
