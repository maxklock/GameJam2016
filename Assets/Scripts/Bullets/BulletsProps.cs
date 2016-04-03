namespace Assets.Scripts.Bullets
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
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
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null && (int)player.Id != _id)
            {
                player.Respawn();
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