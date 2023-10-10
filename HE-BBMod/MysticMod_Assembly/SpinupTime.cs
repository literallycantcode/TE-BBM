using UnityEngine;

namespace MysticFix
{
    [RequireComponent(typeof(CogMotorControllerHinge))]
    public class SpinupTime : MonoBehaviour
    {
        //Basic reference variables
        private BlockBehaviour BB;
        private CogMotorControllerHinge CCH;
        private MSlider Lerpomode;

        //Lerpomode must be preserved during simulation,
        //gets initialized on block's instantiation through the ValueChanged lambda
        private float Lerpo = 0f;

        void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            CCH = GetComponent<CogMotorControllerHinge>();
            Lerpomode = BB.AddSlider("Spin up time", "Lerp", Lerpo, 0f, 10f);
            Lerpomode.ValueChanged += (float value) =>
                {
                    Lerpo = -0.1f + (26.24517f + 0.4575599f) / Mathf.Pow(1f + (value / 2.265289f), 3.291725f);
                    CCH.speedLerpSmooth = Lerpo;
                };
            Lerpomode.DisplayInMapper = true;
        }
    }
}