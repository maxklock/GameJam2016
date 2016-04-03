namespace Assets.Scripts
{
    using System;
    using System.Linq;

    using UnityEngine;

    public class Dugong : MonoBehaviour
    {
        #region methods

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
                player.AddPoints(pearl.Points, "You get " + pearl.Points + " Points");
                Destroy(pearl.gameObject);

                return;
            }

            if (pearl.LastPlayer != PlayerId.None)
            {
                var players = FindObjectsOfType<Player>().Where(p => p.Id == pearl.LastPlayer).ToList();

                if (players.Count > 1)
                {
                    // ReSharper disable once NotResolvedInText
                    throw new ArgumentOutOfRangeException("Player.Id", "There is more than one Player with Id " + pearl.LastPlayer);
                }

                players.First().AddPoints(pearl.Points, "You get "+ pearl.Points + " Points");
            }
            Destroy(pearl.gameObject);
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}