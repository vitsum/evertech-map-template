using UnityEngine;

namespace EvertechMods
{
    public class PushForward : MonoBehaviour
    {
        public float Force;
        public float StopForce;
        public float MaxVelocity;

        private Rigidbody _rb;
        private Transform _tr;

        private void Start()
        {
            _tr = transform;
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            var locVel = transform.InverseTransformDirection(_rb.velocity);
            if (locVel.z < MaxVelocity)
            {
                _rb.AddForce(_tr.forward * Force);
            }
            else
            {
                _rb.AddForce(_tr.forward * -StopForce);
            }
        }
    }
}
