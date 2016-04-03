namespace Assets.Scripts.Ui
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    public class Highscore : MonoBehaviour
    {
        #region methods

        public Text[] TextBoxes;

        public void SetPlaces(string[] place)
        {
            for (var i = 0; i < 4; i++)
            {
                if (i >= place.Length)
                {
                    TextBoxes[i].enabled = false;
                    continue;
                }

                TextBoxes[i].enabled = true;
                TextBoxes[i].text = place[i];
            }
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