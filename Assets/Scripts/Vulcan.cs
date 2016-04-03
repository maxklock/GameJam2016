﻿namespace Assets.Scripts
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public class Vulcan : MonoBehaviour
    {
        #region member vars

        private GameObject _pearlsObject;

        private float _timer;
        public float MaxJumpSpeed = 10.0f;
        public float MaxSpawnSpeed = 5.0f;

        public Pearl[] PearlTypes;
        public Vector3 SpawnOffset;
        public float SpawnRange = 1;

        public float SpawnRate = 2;

        #endregion

        #region methods

        private void SpawnPearl()
        {
            var sum = PearlTypes.Sum(p => p.SpawnRate);
            var rnd = Random.Range(0, sum);
            var pearl = PearlTypes.Last();

            var tmpSum = 0.0f;
            foreach (var p in PearlTypes)
            {
                tmpSum += p.SpawnRate;

                if (tmpSum <= rnd)
                {
                    continue;
                }
                pearl = Instantiate(p);
                break;
            }

            Debug.Log(pearl.name);

            var spawnpos = Random.onUnitSphere * SpawnRange * MaxSpawnSpeed;
            spawnpos.y = 0;

            pearl.transform.position = transform.position + SpawnOffset + spawnpos;
            pearl.transform.parent = _pearlsObject.transform;

            Pearls.Add(pearl);

            spawnpos.y = Random.Range(0, MaxJumpSpeed);

            pearl.Rigidbody.AddForce(spawnpos * 100);
        }

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

        #endregion

        #region properties

        public List<Pearl> Pearls { get; private set; }

        #endregion
    }
}