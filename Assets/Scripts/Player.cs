﻿namespace Assets.Scripts
{
    using Assets.Scripts.Bullets;

    using UnityEngine;

    using UnityStandardAssets.Water;

    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region member vars

        private readonly int _maxBullets = 10;
        private readonly float maxCoolTime = 0.6f;
        private int _bulletsLeft = 10;
        private float _grabTimer;
        private bool _onGround;
        private Transform _pearlParent;
        private float _rCoolTime;
        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private Water _water;

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
        public float MaxCameraDistance = 35;
        public float MinCameraDistance = 4;
        public int Points;
        public float RotationSpeed = 1.0f;
        public float Speed = 5.0f;
        public Rect ViewPort = new Rect(0, 0, 0.5f, 0.5f);

        #endregion

        #region methods

        public void DropPearl()
        {
            _grabTimer = GrabLock;
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
            _pearlParent = pearl.transform.parent;
            pearl.transform.parent = transform;
            pearl.transform.localPosition = GrabOffset;
            pearl.Rigidbody.isKinematic = true;
            pearl.Rigidbody.velocity = Vector3.zero;
            pearl.GetComponent<Collider>().enabled = false;
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

            if (_bulletsLeft < _maxBullets)
            {
                _rCoolTime += Time.deltaTime;
                if (_rCoolTime > maxCoolTime)
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

            transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical Left " + (int)InputType)) * 0.05f * Speed * Time.deltaTime * 60, Space.Self);
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)InputType) * 0.5f * RotationSpeed * Time.deltaTime * 60, 0), Space.Self);

            RotateCamera(Input.GetAxis("Vertical Right " + (int)InputType) * 0.5f * Time.deltaTime * 60, Input.GetAxis("3rd " + (int)InputType) * 0.5f * Time.deltaTime * 60);

            if (_onGround && Input.GetButtonDown("Y " + (int)InputType))
            {
                Rigidbody.AddForce(new Vector3(0, 1000 * JumpSpeed, 0));
            }

            if (Input.GetButtonDown("B " + (int)InputType))
            {
                Shoot();
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