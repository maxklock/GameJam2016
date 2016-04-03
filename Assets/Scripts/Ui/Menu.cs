namespace Assets.Scripts.Ui
{
    using System;

    using UnityEngine;

    public class Menu : MonoBehaviour
    {
        public PlayerSelection PlayerSelection;
        public Game Game;

        public GameObject StateInGame;
        public GameObject StateSelection;

        public GameState State;
        public float GameTime = 3 * 60;

        #region methods

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            StateInGame.SetActive(State == GameState.InGame);
            StateSelection.SetActive(State == GameState.PlayerSelection);

            switch (State)
            {
                case GameState.PlayerSelection:
                    if (PlayerSelection.CanStart)
                    {
                        State = GameState.InGame;
                        Game.PlayerCount = PlayerSelection.PlayerCount;

                        var player = 0;
                        for (var i = 0; i < 4; i++)
                        {
                            if (PlayerSelection.Inputs[i] == InputType.None)
                            {
                                continue;
                            }

                            Game.InputTypes[player] = PlayerSelection.Inputs[i];
                            player++;
                        }
                        Game.InitPlayers();
                    }
                    break;
                case GameState.InGame:
                    if (Time.time - Game.StartTime > GameTime)
                    {
                        State = GameState.PlayerSelection;
                        PlayerSelection.Reset();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}