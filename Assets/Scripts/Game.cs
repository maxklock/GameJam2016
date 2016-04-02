namespace Assets.Scripts
{
    using System;

    using UnityEngine;

    public class Game : MonoBehaviour
    {
        #region member vars

        private GameObject _playerHolder;
        public Player Player;

        [Range(1, 4)]
        public int PlayerCount = 2;

        public Orientation SplitScreen = Orientation.Horizontal;

        public Vector3[] StartPositions = { new Vector3(-70, 0, -70), new Vector3(-70, 0, 70), new Vector3(70, 0, -70), new Vector3(70, 0, 70) };

        #endregion

        #region methods

        // Use this for initialization
        private void Start()
        {
            _playerHolder = new GameObject("Players");

            for (var i = 0; i < PlayerCount; i++)
            {
                var player = Instantiate(Player);
                player.name += " " + (i + 1);
                player.transform.parent = _playerHolder.transform;
                player.Id = (PlayerId)(i + 1);
                player.InputType = (InputType)(i + 1);

                switch (PlayerCount)
                {
                    case 1:
                        player.transform.position = StartPositions[i];
                        player.ViewPort = new Rect(0, 0, 1, 1);
                        break;
                    case 2:
                        player.transform.position = StartPositions[i * 2];
                        player.ViewPort = new Rect(0.5f * i, 0, 0.5f, 1);
                        if (SplitScreen == Orientation.Vertical)
                        {
                            player.ViewPort = new Rect(0, 0.5f * i, 1, 0.5f);
                        }
                        break;
                    case 3:
                    case 4:
                        player.transform.position = StartPositions[i];
                        // ReSharper disable once PossibleLossOfFraction
                        player.ViewPort = new Rect(0.5f * (i / 2), 0.5f * (i % 2), 0.5f, 0.5f);
                        break;
                    default:
                        // ReSharper disable once NotResolvedInText
                        throw new ArgumentOutOfRangeException("PlayerCount", "PlayerCount must be between 1 and 4");
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}