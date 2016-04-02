namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    using UnityStandardAssets.Water;
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region member vars

        public Camera Camera;
        private Transform _pearlParent;
        private Water _water;
        private Vector3 _startPosition;

        private int _maxBullets = 10;
        private int _bulletsLeft = 10;
        private float maxCoolTime =0.6f;
        private float rCoolTime = 0;

        private Rigidbody _rigidbody;
        public float CameraDistance = 10;
        public float MinCameraDistance = 4;
        public float MaxCameraDistance = 35;
        public float CameraDistanceSpeed = 1f;
        public float CameraSpeed = 1f;

        public GameObject BulletsFab;

        public Vector3 LookAtOffset;
        public float CameraRotation = -45;
        public float DropSpeed = 2.0f;

        public Vector3 GrabOffset = new Vector3(0, 0, 1);

        public PlayerId Id;
        public InputType InputType;
        public float RotationSpeed = 1.0f;
        public float Speed = 5.0f;

        public int Points;

        public Rect ViewPort = new Rect(0, 0, 0.5f, 0.5f);

        #endregion

        #region methods

        private void OnCollisionStay(Collision col)
        {
            var pearl = col.transform.GetComponent<Pearl>();
            if (pearl == null)
            {
                return;
            }

            if (GrappedPearl == null && Input.GetButton("A " + (int)Id))
            {
                GrapPearl(pearl);
            }
        }

        public void ResetPosition ()
        {
            this.transform.position = _startPosition;
        }

        public void GrapPearl(Pearl pearl)
        {
            GrappedPearl = pearl;
            _pearlParent = pearl.transform.parent;
            pearl.transform.parent = transform;
            pearl.transform.localPosition = GrabOffset;
            pearl.Rigidbody.isKinematic = true;
            pearl.Rigidbody.velocity = Vector3.zero;
            pearl.GetComponent<Collider>().enabled = false;
        }

        public void DropPearl()
        {
            GrappedPearl.transform.parent = _pearlParent;
            GrappedPearl.Rigidbody.isKinematic = false;
            GrappedPearl.transform.rotation = transform.rotation;
            GrappedPearl.Rigidbody.AddRelativeForce(new Vector3(0, 0, DropSpeed * 100));
            GrappedPearl.GetComponent<Collider>().enabled = true;
            GrappedPearl = null;
        }

        private void RotateCamera(float vertical, float distance)
        {
            Camera.transform.localPosition = Vector3.zero;
            Camera.transform.rotation = transform.rotation;

            CameraDistance += distance * CameraDistanceSpeed;
            CameraDistance = Mathf.Clamp(CameraDistance, MinCameraDistance, MaxCameraDistance);

            CameraRotation += vertical * CameraSpeed;

            Camera.transform.Rotate(Vector3.left, CameraRotation, Space.Self);
            Camera.transform.Translate(new Vector3(0, 0, -CameraDistance), Space.Self);

            Camera.transform.LookAt(transform.position + LookAtOffset);
        }

        public void InitPlayer()
        {
            if (Camera != null)
            {
                DestroyImmediate(Camera);
            }

            transform.LookAt(new Vector3(0, transform.position.y, 0));

            var obj = new GameObject("Camera");
            obj.AddComponent<Camera>();
            Camera = obj.GetComponent<Camera>();
            Camera.rect = ViewPort;
            Camera.transform.parent = transform;

            RotateCamera(0, 0);
        }

        // Use this for initialization
        private void Start()
        {
            Camera = GetComponentInChildren<Camera>();

            _startPosition = transform.position;
            _water = FindObjectOfType<Water>();

        }

        private void Shoot()
        {
            if (_bulletsLeft > 0)
            {
                GameObject obj = GameObject.Instantiate(BulletsFab);
                obj.transform.position = this.transform.position + (this.transform.localRotation * Vector3.forward) + new Vector3(0, 0.5f, 0) ;
                obj.GetComponent<Rigidbody>().velocity = (this.transform.localRotation * Vector3.forward)*60;
                obj.GetComponent<BulletsProps>().Init((int)Id);

                _bulletsLeft--;

            }

        }

        // Update is called once per frame
        private void Update()
        {
			Rigidbody.AddForce(new Vector3(0, -100, 0));

			//if(Input.GetButtonDown("B "+(int)Id))
            if (Input.GetKeyDown(KeyCode.Z))
                Shoot();

            if(_bulletsLeft< _maxBullets)
            {
                rCoolTime += Time.deltaTime;
                if(rCoolTime>maxCoolTime)
                {
                    rCoolTime = 0;
                    _bulletsLeft++;

                }
            }            if (this.transform.position.y < _water.transform.position.y-2)
            {
                this.transform.position = _startPosition;
                ResetPosition();

            }

            transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical Left " + (int)InputType)) * 0.05f * Speed * Time.deltaTime * 60, Space.Self);
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)InputType) * 0.5f * RotationSpeed * Time.deltaTime * 60, 0), Space.Self);

            RotateCamera(Input.GetAxis("Vertical Right " + (int)InputType) * 0.5f * Time.deltaTime * 60, Input.GetAxis("Horizontal Right " + (int)InputType) * 0.5f * Time.deltaTime * 60);

            if (GrappedPearl != null && Input.GetButtonDown("A " + (int)InputType))
            {
                DropPearl();
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