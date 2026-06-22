using UnityEngine;
using UnityEngine.UI;

namespace EvertechMods
{
    public class PushForwardControl : MonoBehaviour
    {
        public PushForward PushForward;
        public Slider Slider;
        public float MinForce = 0;
        public float MaxForce = 7;

        private void Start()
        {
            Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnSliderValueChanged(float value)
        {
            PushForward.Force = MinForce + (MaxForce - MinForce) * value;
            Debug.Log("value: " + value);
        }

        private void OnDestroy()
        {
            Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
