namespace Assets.Scripts.Ui
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerSelection : MonoBehaviour
    {
        #region member vars

        public Image[] Areas = new Image[4];
        public InputType[] Inputs = new InputType[4];
        public bool[] IsReady = new bool[4];

        public int PlayerCount;

        public bool CanStart
        {
            get
            {
                return PlayerCount > 0 && IsReady.Count(r => r) == PlayerCount;
            }
        }

        private const float AlphaNormal = 1.0f;
        private const float AlphaActive = 0.5f;
        private const float AlphaReady = 0.25f;

        #endregion

        #region methods

        public void Reset()
        {
            for (var i = 0; i < 4; i++)
            {
                IsReady[i] = false;
                Areas[i].color = new Color(Areas[i].color.r, Areas[i].color.g, Areas[i].color.b, Inputs[i] == InputType.None ? AlphaNormal : AlphaActive);
            }
        }

        private void Add(InputType input)
        {
            if (!Input.GetButtonDown("A " + (int)input) || Inputs.Any(i => i == input))
            {
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                if (Inputs[i] == InputType.None)
                {
                    PlayerCount++;
                    Inputs[i] = input;
                    Areas[i].color = new Color(Areas[i].color.r, Areas[i].color.g, Areas[i].color.b, AlphaActive);
                    break;
                }
            }
        }

        private void Remove(InputType input)
        {
            if (!Input.GetButtonDown("B " + (int)input) || !Inputs.Any(i => i == input))
            {
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                if (Inputs[i] == input)
                {
                    PlayerCount--;
                    Inputs[i] = InputType.None;
                    Areas[i].color = new Color(Areas[i].color.r, Areas[i].color.g, Areas[i].color.b, AlphaNormal);
                    break;
                }
            }
        }

        private void Ready(InputType input)
        {
            if (!Input.GetButtonDown("X " + (int)input) || !Inputs.Any(i => i == input))
            {
                return;
            }

            for (var i = 0; i < 4; i++)
            {
                if (Inputs[i] == input)
                {
                    IsReady[i] = !IsReady[i];
                    Areas[i].color = new Color(Areas[i].color.r, Areas[i].color.g, Areas[i].color.b, IsReady[i] ? AlphaReady : AlphaActive);
                    break;
                }
            }
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            foreach (var input in Enum.GetValues(typeof(InputType)).Cast<InputType>().Where(i => i != InputType.None))
            {
                Add(input);
                Ready(input);
                Remove(input);
            }
        }

        #endregion
    }
}