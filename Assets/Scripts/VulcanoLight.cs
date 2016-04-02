namespace Assets.Scripts
{
    using UnityEngine;

    public class VulcanoLight : MonoBehaviour
    {
        #region member vars

        private Light _pointLight;
        private float _rf;

        #endregion

        #region methods

        // Use this for initialization
        private void Start()
        {
            _pointLight = GetComponent<Light>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Random.value > 0.8)
            {
                _rf += Random.value / 2;
            }

            _pointLight.intensity = 6 + Mathf.Sin(Time.realtimeSinceStartup + _rf) * 3;
        }

        #endregion
    }
}