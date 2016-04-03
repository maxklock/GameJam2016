namespace Assets.Scripts
{
    using System;
    using System.Linq;

    using UnityEngine;

    public class Dugong : MonoBehaviour
    {
        ParticleSystem p_system;

        int maxPearls = 3;
        int pearlCount = 0;

        float maxHungerTime = 66;
        float rHungerTime = 0;

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
                if (pearlCount < maxPearls)
                {
                    pearl = player.GrappedPearl;
                    player.DropPearl();
                    player.AddPoints(pearl.Points, "You get " + pearl.Points + " Points");
                    pearlCount++;
                    Destroy(pearl.gameObject);
                }
                else
                {
                    player.AddMessage("Dugong is stuffed!");
                }

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
                if (pearlCount < maxPearls)
                {
                    players.First().AddPoints(pearl.Points, "You get " + pearl.Points + " Points");
                    pearlCount++;
                }
            }
            Destroy(pearl.gameObject);
        }

        // Use this for initialization
        private void Start()
        {

            p_system = GetComponentInChildren<ParticleSystem>();
        }

        // Update is called once per frame
        private void Update()
        {
            rHungerTime += Time.deltaTime;

            if(rHungerTime > maxHungerTime)
            {
                if(pearlCount>0)
                    pearlCount--;
                rHungerTime = 0;
            }

            if (pearlCount >= maxPearls)
                p_system.enableEmission = false;
            else
            {
                p_system.enableEmission = true;
            }
        }

        #endregion
    }
}