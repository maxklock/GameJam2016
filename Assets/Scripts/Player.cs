namespace Assets.Scripts
{
    using System.Linq;

    using Assets.Scripts.Bullets;

    using UnityEngine;

    using UnityStandardAssets.Water;

    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region member vars

        private int _bulletsLeft = 10;
        private float _grabTimer;
        private bool _onGround;
        private Transform _pearlParent;
        private float _rCoolTime;
        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private Water _water;
        public float BulletCoolTime = 0.6f;

        public GameObject BulletsFab;
        public Camera Camera;
        public float CameraDistance = 10;
        public float CameraDistanceSpeed = 1f;
        public float CameraRotation = -45;
        public float CameraSpeed = 1f;
        public float DropSpeed = 2.0f;
        public float GrabLock = 2.0f;
        public Vector3 GrabOffset = new Vector3(0, 0, 1);
        public PlayerId Id;
        public InputType InputType;
        public float JumpSpeed = 5.0f;
        public Vector3 LookAtOffset;

        public int MaxBullets = 10;
        public float MaxCameraDistance = 35;
        public float MinCameraDistance = 4;
        public int Points;
        public float PushDistance = 2.0f;
        public float RotationSpeed = 1.0f;
        public float Speed = 5.0f;
        public Rect ViewPort = new Rect(0, 0, 0.5f, 0.5f);

        #endregion

        #region constants

        private const float JumpFactor = 1000;

        #endregion

        #region methods

        public void DropPearl()
        {
            _grabTimer = GrabLock;
            GrappedPearl.Drop();
            GrappedPearl.transform.parent = _pearlParent;
            GrappedPearl.Rigidbody.isKinematic = false;
            GrappedPearl.transform.rotation = transform.rotation;
            GrappedPearl.Rigidbody.AddRelativeForce(new Vector3(0, 0, DropSpeed * 100));
            GrappedPearl.GetComponent<Collider>().enabled = true;
            GrappedPearl = null;
        }

        public void GrapPearl(Pearl pearl)
        {
            if (_grabTimer > 0)
            {
                return;
            }
            GrappedPearl = pearl;
            _pearlParent = GrappedPearl.transform.parent;
            GrappedPearl.Grab();
            GrappedPearl.transform.parent = transform;
            GrappedPearl.transform.localPosition = GrabOffset;
            GrappedPearl.Rigidbody.isKinematic = true;
            GrappedPearl.Rigidbody.velocity = Vector3.zero;
            GrappedPearl.GetComponent<Collider>().enabled = false;
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

        public void PushPearl(Vector3 source)
        {
            _grabTimer = GrabLock;
            GrappedPearl.Drop();
            GrappedPearl.transform.parent = _pearlParent;
            GrappedPearl.Rigidbody.isKinematic = false;
            GrappedPearl.transform.rotation = transform.rotation;
            var dir = transform.position - source;
            dir.Normalize();
            GrappedPearl.Rigidbody.AddForce(dir * DropSpeed * 100);
            GrappedPearl.GetComponent<Collider>().enabled = true;
            GrappedPearl = null;
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
        }

        private void OnCollisionEnter(Collision col)
        {
            var terrain = col.gameObject.GetComponent<Terrain>();
            if (terrain == null)
            {
                return;
            }

            _onGround = true;
        }

        private void OnCollisionExit(Collision col)
        {
            var terrain = col.gameObject.GetComponent<Terrain>();
            if (terrain == null)
            {
                return;
            }

            _onGround = false;
        }

        private void OnCollisionStay(Collision col)
        {
            var pearl = col.transform.GetComponent<Pearl>();
            if (pearl == null)
            {
                var terrain = col.gameObject.GetComponent<Terrain>();
                if (terrain == null)
                {
                    return;
                }

                _onGround = col.contacts.Any(c => c.point.y < transform.position.y + 0.01f);

                return;
            }

            if (GrappedPearl == null && Input.GetButton("A " + (int)InputType))
            {
                GrapPearl(pearl);
            }
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

        private void Shoot()
        {
            if (_bulletsLeft > 0)
            {
                GameObject obj = Instantiate(BulletsFab);
                obj.transform.position = transform.position + (transform.localRotation * Vector3.forward) + new Vector3(0, 0.5f, 0);
                obj.GetComponent<Rigidbody>().velocity = (transform.localRotation * Vector3.forward) * 60;
                obj.GetComponent<BulletsProps>().Init((int)Id);

                _bulletsLeft--;
            }
        }

        // Use this for initialization
        private void Start()
        {
            Camera = GetComponentInChildren<Camera>();

            _startPosition = transform.position;
            _water = FindObjectOfType<Water>();

            _grabTimer = -1;
        }

        // Update is called once per frame
        private void Update()
        {
            _grabTimer -= Time.deltaTime;
            if (_grabTimer < -1)
            {
                _grabTimer = -1;
            }
            Rigidbody.AddForce(new Vector3(0, -100, 0));

            if (_bulletsLeft < MaxBullets)
            {
                _rCoolTime += Time.deltaTime;
                if (_rCoolTime > BulletCoolTime)
                {
                    _rCoolTime = 0;
                    _bulletsLeft++;
                }
            }
            if (transform.position.y < _water.transform.position.y - 2)
            {
                transform.position = _startPosition;
                ResetPosition();
            }

            var vertInput = Input.GetAxis("Vertical Left " + (int)InputType);
            var ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
            RaycastHit hit;
            var front = Physics.Raycast(ray, out hit, 1.0f);
            ray.direction *= -1;
            var back = Physics.Raycast(ray, out hit, 1.0f);

            if ((!front || vertInput < 0) && (!back || vertInput > 0))
            {
                transform.Translate(new Vector3(0, 0, vertInput) * 0.05f * Speed * Time.deltaTime * 60, Space.Self);
            }

            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)InputType) * 0.5f * RotationSpeed * Time.deltaTime * 60, 0), Space.Self);

            RotateCamera(Input.GetAxis("Vertical Right " + (int)InputType) * 0.5f * Time.deltaTime * 60, Input.GetAxis("3rd " + (int)InputType) * 0.5f * Time.deltaTime * 60);

            if (_onGround && Input.GetButtonDown("Y " + (int)InputType))
            {
                Rigidbody.AddForce(new Vector3(0, JumpFactor * JumpSpeed, 0));
            }

            if (Input.GetButtonDown("B " + (int)InputType))
            {
                Shoot();
            }

            if (Input.GetButtonDown("X " + (int)InputType))
            {
                ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);

                Debug.DrawRay(ray.origin, ray.direction);

                if (Physics.Raycast(ray, out hit, PushDistance))
                {
                    var player = hit.collider.gameObject.GetComponentInParent<Player>();
                    if (player != null)
                    {
                        player.PushPearl(transform.position);
                    }
                }
            }

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