namespace Assets.Scripts.Bullets
{
    using UnityEngine;

    public class BulletsProps : MonoBehaviour
    {
        #region member vars

        private int _id;
        private float _rLifeTime;

        public float MaxLifeTime = 2;

        #endregion

        #region methods

        public void Init(int playerId)
        {
            _id = playerId;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Player>() != null && (int)collision.gameObject.GetComponent<Player>().Id != _id)
            {
                collision.gameObject.GetComponent<Player>().ResetPosition();
            }
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            _rLifeTime += Time.deltaTime;

            if (_rLifeTime > MaxLifeTime)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}