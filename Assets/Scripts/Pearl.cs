using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pearl : MonoBehaviour
    {
        #region methods

        public float MaxDepth = -10;

        private Rigidbody _rigidbody;

        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody = _rigidbody ?? GetComponent<Rigidbody>();
            }
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.position.y < MaxDepth)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}