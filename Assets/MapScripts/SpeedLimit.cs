using UnityEngine;

namespace EvertechMods
{
    public class SpeedLimit : MonoBehaviour
    {
        public float Speed = 10;

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Train")
            {
                other.GetComponent<PushForward>().MaxVelocity = Speed;
            }
        }
    }
}
