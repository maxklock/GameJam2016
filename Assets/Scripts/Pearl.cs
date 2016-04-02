namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Pearl : MonoBehaviour
    {
        #region member vars

        private Renderer _renderer;

        private Rigidbody _rigidbody;
        private float _startLifeTime;
        public float LifeTime = 30.0f;

        public float MaxDepth = -10;

        [Range(0.0f, 1.0f)]
        public float MinAlpha = 0.25f;

        #endregion

        #region methods

        public void Drop()
        {
            IsGrabbed = false;
        }

        public void Grab()
        {
            IsGrabbed = true;
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

        public bool IsGrabbed { get; private set; }

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