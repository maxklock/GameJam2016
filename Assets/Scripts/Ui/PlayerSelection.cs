namespace Assets.Scripts.Ui
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerSelection : MonoBehaviour
    {
        #region member vars

        public Sprite ImgNormal;
        public Sprite[] ImgPads;
        public Sprite ImgKeyboard;
        public Sprite ImgReady;

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

        #endregion

        #region methods

        public void Reset()
        {
            for (var i = 0; i < 4; i++)
            {
                IsReady[i] = false;
                Areas[i].sprite = InputToSprite(Inputs[i]);
            }
        }

        private Sprite InputToSprite(InputType input)
        {
            if (input == InputType.None)
            {
                return ImgNormal;
            }

            if (input == InputType.Keyboard)
            {
                return ImgKeyboard;
            }

            return ImgPads[(int)input - 1];
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
                    Areas[i].sprite = InputToSprite(input);
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
                    Areas[i].sprite = ImgNormal;
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
                    Areas[i].sprite = IsReady[i] ? ImgReady : InputToSprite(input);
                    break;
                }
            }
        }

        // Use this for initialization
        private void Start()
        {
            Reset();
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