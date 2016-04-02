﻿namespace Assets.Scripts
{
    using System;

    using UnityEditor;

    using UnityEngine;

    public class Game : MonoBehaviour
    {
        #region member vars

        private GameObject _playerHolder;
        public Player Player;
        public int PlayerCount = 2;
        public Orientation SplitScreen = Orientation.Horizontal;

        public Vector3[] StartPositions = { new Vector3(-70, 0, -70), new Vector3(70, 0, 70), new Vector3(70, 0, -70), new Vector3(-70, 0, 70) };
        public InputType[] InputTypes = { InputType.Joystick1, InputType.Joystick2, InputType.Joystick3, InputType.Keyboard,  };

        #endregion

        #region methods

        public void InitPlayers()
        {
            if (_playerHolder != null)
            {
                DestroyImmediate(_playerHolder);
            }

            _playerHolder = new GameObject("Players");

            var players = _playerHolder.GetComponentsInChildren<Player>();
            foreach (var player in players)
            {
                DestroyImmediate(player.gameObject);
            }

            for (var i = 0; i < PlayerCount; i++)
            {
                var player = Instantiate(Player);
                player.name += " " + (i + 1);
                player.transform.parent = _playerHolder.transform;
                player.Id = (PlayerId)(i + 1);
                player.InputType = InputTypes[i];

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