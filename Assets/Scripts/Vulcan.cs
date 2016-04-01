namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class Vulcan : MonoBehaviour
    {
        #region member vars
        
        public float SpawnRate = 2;
        public Vector3 SpawnOffset;
        public float SpawnRange = 1;
        public float MaxSpawnSpeed = 5.0f;
        public float MaxJumpSpeed = 10.0f;

        public Pearl[] PearlTypes;

        public List<Pearl> Pearls { get; private set; }

        private float _timer;
        private GameObject _pearlsObject;

        #endregion

        #region methods

        // Use this for initialization
        private void Start()
        {
            _timer = 0;
            Pearls = new List<Pearl>();
            _pearlsObject = new GameObject("Pearls");
        }

        // Update is called once per frame
        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer > 1 / SpawnRate)
            {
                _timer = 0;
                SpawnPearl();
            }
        }

        private void SpawnPearl()
        {
            var rnd = Random.Range(0, PearlTypes.Length);
            var pearl = Instantiate(PearlTypes[rnd]);

            var spawnpos = Random.onUnitSphere * SpawnRange;
            spawnpos.y = 0;

            pearl.transform.position = transform.position + SpawnOffset + spawnpos;
            pearl.transform.parent = _pearlsObject.transform;

            Pearls.Add(pearl);

            spawnpos.y = Random.Range(0, MaxJumpSpeed);

            pearl.Rigidbody.AddForce(spawnpos * 100);
        }

        #endregion
    }
}