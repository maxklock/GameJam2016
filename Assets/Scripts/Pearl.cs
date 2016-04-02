namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Pearl : MonoBehaviour
    {
        #region member vars

        private Rigidbody _rigidbody;
        private Renderer _renderer;
        private float _startLifeTime;

        public float MaxDepth = -10;
        public float LifeTime = 30.0f;

        [Range(0.0f, 1.0f)]
        public float MinAlpha = 0.25f;

        public bool IsGrabbed { get; private set; }

        #endregion

        #region methods

        public void Grab()
        {
            IsGrabbed = true;
        }

        public void Drop()
        {
            IsGrabbed = false;
        }

        // Use this for initialization
        private void Start()
        {
            IsGrabbed = false;
            _startLifeTime = LifeTime;
            _renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.position.y < MaxDepth)
            {
                Destroy(gameObject);
            }

            if (!IsGrabbed)
            {
                LifeTime -= Time.deltaTime;

                var color = _renderer.material.color;
                _renderer.material.color = new Color(color.r, color.g, color.b, (LifeTime / _startLifeTime) * (1 - MinAlpha) + MinAlpha);

                if (LifeTime < 0)
                {
                    Destroy(gameObject);
                }
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