using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region methods

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get
            {
                return _rigidbody = _rigidbody ?? GetComponent<Rigidbody>();
            }
        }

        private Camera _camera;

        public PlayerId Id;
        public float Speed = 5.0f;
        public float RotationSpeed = 1.0f;

        // Use this for initialization
        private void Start()
        {
            var obj = new GameObject("Camera");
            obj.AddComponent<Camera>();
            _camera = obj.GetComponent<Camera>();
            _camera.rect = new Rect((((int)Id - 1) / 2) * 0.5f, (((int)Id - 1) % 2) * 0.5f, 0.5f, 0.5f);
            _camera.transform.parent = transform;
            _camera.transform.localPosition = new Vector3(0, 1, -2);
        }

        // Update is called once per frame
        private void Update()
        {
            Rigidbody.AddRelativeForce(new Vector3(0, 0, Input.GetAxis("Vertical Left " + (int)Id)) * 10 * Speed);
            Rigidbody.AddRelativeTorque(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)Id) * RotationSpeed, 0));
        }

        #endregion
    }
}