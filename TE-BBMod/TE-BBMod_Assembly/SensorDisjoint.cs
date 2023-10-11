using System;
using UnityEngine;
using Modding;
using Modding.Blocks;
using System.Collections.Generic;
using Console = Modding.ModConsole;

namespace MysticFix
{
    public class SensorDisjoint : MonoBehaviour
    {
        private BlockBehaviour BB;
        private MSlider DS;
        private Transform SensorPos;
        private float defaultAmount = 0;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();
            SensorPos = BB.transform.FindChild("SensorPos");
            defaultAmount = SensorPos.localPosition.y;
            DS = BB.AddSlider("Disjoint", "Disjoint", -defaultAmount, 0f, 4f);
            DS.ValueChanged += (value => 
            {
                SensorPos.localPosition = new Vector3(SensorPos.localPosition.x, -value, SensorPos.localPosition.z);
            });
        }
    }
}