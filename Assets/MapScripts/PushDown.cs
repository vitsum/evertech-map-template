using UnityEngine;

namespace EvertechMods
{
    public class PushDown : MonoBehaviour
    {
        public float DownForce;

        private Rigidbody _rb;
        private Transform _tr;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _tr = transform;
        }

        void FixedUpdate()
        {
            _rb.AddForce(_tr.up * -DownForce);
        }
    }
}
