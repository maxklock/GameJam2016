namespace Assets.Scripts
{
    using UnityEngine;

    public class Dugong : MonoBehaviour
    {
        #region methods

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        private void OnCollisionEnter(Collision col)
        {
            var pearl = col.gameObject.GetComponent<Pearl>();
            var player = col.gameObject.GetComponent<Player>();

            if (pearl == null && player == null)
            {
                return;
            }

            if (player != null)
            {
                if (player.GrappedPearl == null)
                {
                    return;
                }

                pearl = player.GrappedPearl;
                player.DropPearl();
                player.Points++;
                Destroy(pearl.gameObject);

                return;
            }

            Destroy(pearl.gameObject);
        }

        #endregion
    }
}