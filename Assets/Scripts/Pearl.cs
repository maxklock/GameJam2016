namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Pearl : MonoBehaviour
    {
        #region member vars

        private Rigidbody _rigidbody;

        public float MaxDepth = -10;

        #endregion

        #region methods

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

        #region properties

        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody = _rigidbody ?? GetComponent<Rigidbody>();
            }
        }

        #endregion
    }
}