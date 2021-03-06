﻿namespace Assets.Scripts
{
    using System.Linq;

    using Assets.Scripts.Bullets;
    using Assets.Scripts.Ui;

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
        private Animator animator;
        public float BulletCoolTime = 0.6f;
        private float maxRespawn = 0.6f;
        private float rRespawn = 0;
        private bool isRespawning = false;
        private float rollFastSpeed = 2;
        private float rollSpeed = 1;
        private float maxRollTime = 1;
        private float rRollTime= 0;
        private bool rolling = false;
        private bool boost = true;
        private float boostVal = 1;
        private float boostTime = 0;
        private float maxboostTime = 3;

        public BulletsProps BulletsFab;
        public Camera Camera;
        public float CameraDistance = 15;
        public float CameraDistanceSpeed = 1f;
        public float CameraRotation = 35;
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
        public float MinCameraDistance = 5;
        public float MinCameraRotation = 15.0f;
        public float MaxCameraRotation = 80.0f;
        public int Points;
        public float PushDistance = 2.0f;
        public float RotationSpeed = 1.0f;
        public float Speed = 5.0f;
        public Rect ViewPort = new Rect(0, 0, 0.5f, 0.5f);

        public PlayerUi PlayerUi;
        public Game Game;

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

        public void AddPoints(int value, string message = "")
        {
            SetPoints(Points + value);
            AddMessage(message);
        }

        public void SetPoints(int value)
        {
            Points = value;
            PlayerUi.TbxPoints.text = Points.ToString("0");
        }

        public void GrapPearl(Pearl pearl)
        {
            if (_grabTimer > 0)
            {
                return;
            }
            GrappedPearl = pearl;
            _pearlParent = GrappedPearl.transform.parent;
            GrappedPearl.Grab(Id);
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

            PlayerUi.transform.parent = transform;
            PlayerUi.Root.worldCamera = Camera;
            PlayerUi.Root.planeDistance = 1;

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

        public void Respawn()
        {
            animator.SetBool("respawn", true);
            isRespawning = true;
            AddPoints(-1 , "You died! (-1 Point)");
            transform.position = Game.StartPositions[Random.Range(0, Game.StartPositions.Length)];


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

                _onGround = col.contacts.Any(c => c.point.y < transform.position.y + 0.31f);

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

            CameraRotation -= vertical * CameraSpeed;
            CameraRotation = Mathf.Clamp(CameraRotation, MinCameraRotation, MaxCameraRotation);

            Camera.transform.Rotate(Vector3.left, -CameraRotation, Space.Self);
            Camera.transform.Translate(new Vector3(0, 0, -CameraDistance), Space.Self);

            Camera.transform.LookAt(transform.position + LookAtOffset);
        }

        private void Shoot()
        {
            if (_bulletsLeft > 0)
            {
                var obj = Instantiate(BulletsFab);
                obj.transform.position = transform.position + (transform.localRotation * Vector3.forward) + new Vector3(0, 0.5f, 0);
                obj.GetComponent<Rigidbody>().velocity = (transform.localRotation * Vector3.forward) * 60;
                obj.Init((int)Id);

                _bulletsLeft--;
            }
        }

        // Use this for initialization
        private void Start()
        {
            Camera = GetComponentInChildren<Camera>();
            animator = GetComponentInChildren<Animator>();

            _startPosition = transform.position;
            _water = FindObjectOfType<Water>();

            _grabTimer = -1;
        }

        public void UpdateTime()
        {
            var diff = Time.time - Game.StartTime;
            var minutes = (int)(diff / 60);
            var seconds = ((int)diff) % 60;

            PlayerUi.TbxTime.text = minutes.ToString("0") + ":" + seconds.ToString("00");
        }

        public void AddMessage(string message)
        {
            PlayerUi.AddMessage(message);
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateTime();

            if (rolling)
            {
                rRollTime += Time.deltaTime;
                rollSpeed = rollFastSpeed;

                if (rRollTime > maxRollTime)
                {
                    rRollTime = 0;
                    rolling = false;
                    rollSpeed = 1;

                }

            }


            if (boost)
            {
                boostTime += Time.deltaTime;

                if (boostTime > maxboostTime)
                {
                    boostTime = 0;
                    boost = false;
                    boostVal = 1;

                }

            }

            if (isRespawning)
            {
                animator.SetBool("idle_long", true);
                animator.SetBool("jumping", true);
                animator.SetBool("running", true);
                rRespawn += Time.deltaTime;

                //Body
                SkinnedMeshRenderer[] sk = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                Color c = sk[0].material.color;
                sk[0].material.color = new Color(c.r, c.g, c.b, rRespawn/maxRespawn);

                // Alles andere
                for (int i = 1; i < sk.Length; i++)
                {
                    Color cx = sk[i].material.color;
                    sk[i].material.color = new Color(cx.r, cx.g, cx.b, 0.001f);
                }   



                if (rRespawn>maxRespawn)
                {
                    rRespawn = 0;
                    isRespawning = false;
                    animator.SetBool("respawn", false);

                }
            }
            else
            {
                for (int i = 0; i < gameObject.GetComponentsInChildren<SkinnedMeshRenderer>().Length; i++)
                {
                    Color cx = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[i].material.color;
                    gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[i].material.color = new Color(cx.r, cx.g, cx.b, 1);
                }
            }

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
            if (transform.position.y < _water.transform.position.y - 4)
            {
                //transform.position = _startPosition;
                Respawn();
            }

            var vertInput = Input.GetAxis("Vertical Left " + (int)InputType);
            var ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);
            RaycastHit hit;
            var front = Physics.Raycast(ray, out hit, 1.0f);
            ray.direction *= -1;
            var back = Physics.Raycast(ray, out hit, 1.0f);

            


            if ((!front || vertInput < 0) && (!back || vertInput > 0) && !isRespawning)
            {

                transform.Translate(new Vector3(0, 0, vertInput) * 0.05f * Speed * rollSpeed *boostVal* Time.deltaTime * 60, Space.Self);
                animator.SetBool("running", true);
                if (Mathf.Abs(vertInput) < 0.1f)
                animator.SetBool("running", false);

                if (Random.Range(0, 100) > 80)
                    animator.SetBool("idle_long", true);
                else
                    animator.SetBool("idle_long", false);
                animator.SetBool("jumping", false);
            }
            if (!isRespawning)
            transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal Left " + (int)InputType) * 0.5f * RotationSpeed * Time.deltaTime * 60, 0), Space.Self);

            RotateCamera(Input.GetAxis("Vertical Right " + (int)InputType) * 0.5f * Time.deltaTime * 60, Input.GetAxis("3rd " + (int)InputType) * 0.5f * Time.deltaTime * 60);

            if (_onGround && Input.GetButtonDown("Y " + (int)InputType)&& !isRespawning)
            {

                Rigidbody.AddForce(new Vector3(0, JumpFactor * JumpSpeed, 0));
                animator.SetBool("jumping", true);
            }

            if (Input.GetButtonDown("B " + (int)InputType) && !isRespawning)
            {
                Shoot();
            }
            animator.SetBool("roll", false);
            if (_onGround && Input.GetButtonDown("X " + (int)InputType) && !isRespawning)
            {


                rolling = true;
                if(GrappedPearl!=null)
                    DropPearl();
                animator.SetBool("roll", true);


            }

            if (GrappedPearl != null && Input.GetButtonDown("A " + (int)InputType) && !isRespawning)
            {
                DropPearl();
            }
        }

        public void StartBoost()
        {
            boost = true;
            boostVal = 2;
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