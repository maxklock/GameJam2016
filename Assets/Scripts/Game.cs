﻿namespace Assets.Scripts
{
    using System;
    using System.Linq;

    using UnityEngine;

    [RequireComponent(typeof(UiManager))]
    public class Game : MonoBehaviour
    {
        #region member vars

        public bool AllowEmptyViewPort = true;
        public InputType[] InputTypes = { InputType.Joystick1, InputType.Joystick2, InputType.Joystick3, InputType.Keyboard };
        public Player Player;
        public int PlayerCount = 2;
        public Orientation SplitScreen = Orientation.Horizontal;

        public Vector3[] StartPositions = { new Vector3(-60, 0, -60), new Vector3(60, 0, 60), new Vector3(60, 0, -60), new Vector3(-60, 0, 60) };

        public UiManager UiManager;

        #endregion

        #region methods

        public void InitPlayers()
        {
            UiManager = GetComponent<UiManager>();

            var players = GetComponentsInChildren<Player>();
            foreach (var player in players)
            {
                DestroyImmediate(player.gameObject);
            }

            UiManager.ClearPlayerUis();

            for (var i = 0; i < PlayerCount; i++)
            {
                var player = Instantiate(Player);
                player.name += " " + (i + 1);
                player.transform.parent = transform;
                player.Id = (PlayerId)(i + 1);
                player.InputType = InputTypes[i];
                player.PlayerUi = UiManager.GetPlayerUi(player.Id);

                switch (PlayerCount)
                {
                    case 1:
                        player.transform.position = StartPositions[i];
                        player.ViewPort = new Rect(0, 0, 1, 1);
                        break;
                    case 2:
                        player.transform.position = StartPositions[i];
                        player.ViewPort = new Rect(0.5f * i, 0, 0.5f, 1);
                        if (SplitScreen == Orientation.Vertical)
                        {
                            player.ViewPort = new Rect(0, 0.5f * i, 1, 0.5f);
                        }
                        break;
                    case 3:
                        player.transform.position = StartPositions[i];
                        if (!AllowEmptyViewPort)
                        {
                            player.ViewPort = new Rect(0.3333f * i, 0, 0.3333f, 1);
                            if (SplitScreen == Orientation.Vertical)
                            {
                                player.ViewPort = new Rect(0, 0.3333f * i, 1, 0.3333f);
                            }
                            break;
                        }
                        // ReSharper disable once PossibleLossOfFraction
                        player.ViewPort = new Rect(0.5f * (i / 2), 0.5f * (i % 2), 0.5f, 0.5f);
                        break;
                    case 4:
                        player.transform.position = StartPositions[i];
                        // ReSharper disable once PossibleLossOfFraction
                        player.ViewPort = new Rect(0.5f * (i / 2), 0.5f * (i % 2), 0.5f, 0.5f);
                        break;
                    default:
                        // ReSharper disable once NotResolvedInText
                        throw new ArgumentOutOfRangeException("PlayerCount", "PlayerCount must be between 1 and 4");
                }

                player.InitPlayer();
                player.SetPoints(0);
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