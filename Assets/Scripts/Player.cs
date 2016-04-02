namespace Assets.Scripts
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region member vars

        private Camera _camera;
        private Transform _pearlParent;

        private Rigidbody _rigidbody;
        public int SelectedCameraOffset = 0;
        public Vector3[] CameraOffsets =
        {
            new Vector3(0, 1, -2),
            new Vector3(0, 2, -4),
            new Vector3(0, 3, -6)
        };

        public Vector3 LookAtOffset;
        public Vector2 CameraRotation = new Vector2(-20, 0);
        public float DropSpeed = 2.0f;

        public Vector3 GrabOffset = new Vector3(0, 0, 1);

        public PlayerId Id;
        public InputType InputType;
        public float RotationSpeed = 1.0f;
        public float Speed = 5.0f;

        public Rect ViewPort = new Rect(0, 0, 0.5f, 0.5f);

        #endregion

        #region methods

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

        private void RotateCamera(float vertical, float horizontal)
        {
            _camera.transform.localPosition = Vector3.zero;
            _camera.transform.rotation = transform.rotation;

            CameraRotation.y += horizontal;
            CameraRotation.x += vertical;

            _camera.transform.Rotate(Vector3.up, CameraRotation.y, Space.Self);
            _camera.transform.Rotate(Vector3.left, CameraRotation.x, Space.Self);
            _camera.transform.Translate(CameraOffsets[SelectedCameraOffset], Space.Self);

            _camera.transform.LookAt(transform.position + LookAtOffset);
        }

        public void InitPlayer()
        {
            if (_camera != null)
            {
                DestroyImmediate(_camera);
            }

            transform.LookAt(new Vector3(0, transform.position.y, 0));

            var obj = new GameObject("Camera");
            obj.AddComponent<Camera>();
            _camera = obj.GetComponent<Camera>();
            _camera.rect = ViewPort;
            _camera.transform.parent = transform;

            RotateCamera(0, 0);
        }

        // Use this for initialization
        private void Start()
        {
            _camera = GetComponentInChildren<Camera>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Mathf.Abs(Input.GetAxis("Vertical Left " + (int)InputType)) <= 0.01)
            {
                Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal Left " + (int)InputType)) <= 0.01)
            {
                Rigidbody.angularVelocity = new Vector3(0, 0, 0);
            }

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

            if (Input.GetButtonDown("B " + (int)InputType))
            {
                SelectedCameraOffset = (SelectedCameraOffset + 1) % CameraOffsets.Length;
            }
        }

        #endregion

        #region properties

        public Pearl GrappedPearl { get; private set; }

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