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
        private Transform _pearlParent;

        public Pearl GrappedPearl { get; private set; }

        public PlayerId Id;
        public InputType InputType;
        public float Speed = 5.0f;
        public float RotationSpeed = 1.0f;
        public float DropSpeed = 10.0f;

        public Vector3 GrabOffset = new Vector3(0, 0, 1);
        public Vector3 CameraOffset = new Vector3(0, 1, -2);
        public Vector2 CameraRotation = new Vector2(-30, 0);

        // Use this for initialization
        private void Start()
        {
            var obj = new GameObject("Camera");
            obj.AddComponent<Camera>();
            _camera = obj.GetComponent<Camera>();
            _camera.rect = new Rect((((int)Id - 1) / 2) * 0.5f, (((int)Id - 1) % 2) * 0.5f, 0.5f, 0.5f);
            _camera.transform.parent = transform;
            _camera.transform.localPosition = CameraOffset;
        }

        // Update is called once per frame
        private void Update()
        {
            Rigidbody.AddRelativeForce(new Vector3(0, 0, Input.GetAxis("Vertical Left " + (int)InputType)) * 10 * Speed);
            Rigidbody.AddRelativeTorque(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)InputType) * RotationSpeed, 0));

            RotateCamera(Input.GetAxis("Vertical Right " + (int)InputType), Input.GetAxis("Horizontal Right " + (int)InputType));

            if (GrappedPearl != null && Input.GetButtonDown("A " + (int)InputType))
            {
                GrappedPearl.transform.parent = _pearlParent;
                GrappedPearl.Rigidbody.isKinematic = false;
                GrappedPearl.transform.rotation = transform.rotation;
                GrappedPearl.Rigidbody.AddRelativeForce(new Vector3(0, 0, DropSpeed * 100));
                GrappedPearl.GetComponent<Collider>().enabled = true;
                GrappedPearl = null;
            }
        }

        private void RotateCamera(float vertical, float horizontal)
        {
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.rotation = transform.rotation;

            CameraRotation.y += horizontal;
            CameraRotation.x += vertical;

            _camera.transform.Rotate(Vector3.up, CameraRotation.y, Space.Self);
            _camera.transform.Rotate(Vector3.left, CameraRotation.x, Space.Self);
            _camera.transform.LookAt(transform);
            _camera.transform.Translate(CameraOffset, Space.Self);
        }

        private void OnCollisionStay(Collision col)
        {
            var pearl = col.transform.GetComponent<Pearl>();
            if (pearl != null && GrappedPearl == null && Input.GetButton("A " + (int)Id))
            {
                GrappedPearl = pearl;
                _pearlParent = pearl.transform.parent;
                pearl.transform.parent = transform;
                pearl.transform.localPosition = GrabOffset;
                pearl.Rigidbody.isKinematic = true;
                pearl.Rigidbody.velocity = Vector3.zero;
                pearl.GetComponent<Collider>().enabled = false;
            }
        }

        #endregion
    }
}