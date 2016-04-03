namespace Assets.Scripts.Ui
{
    using System;
    using System.Linq;

    using UnityEngine;

    public class Menu : MonoBehaviour
    {
        public PlayerSelection PlayerSelection;
        public Game Game;
        public Highscore Highscore;

        public GameObject StateInGame;
        public GameObject StateSelection;
        public GameObject StateGameOver;

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
            StateGameOver.SetActive(State == GameState.GameOver);

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
                        State = GameState.GameOver;
                        var places = new string[Game.PlayerCount];
                        var players = FindObjectsOfType<Player>().ToList();
                        players.Sort((p1, p2) => p2.Points - p1.Points);
                        for (var i = 0; i < places.Length; i++)
                        {
                            places[i] = players[i].Id.ToString() + " (" + players[i].Points + ")";
                        }
                        Highscore.SetPlaces(places);
                        PlayerSelection.Reset();
                    }
                    break;
                case GameState.GameOver:
                    if (Enum.GetValues(typeof(InputType)).Cast<InputType>().Where(i => i != InputType.None).Any(input => Input.GetButtonDown("A " + (int)input)))
                    {
                        State = GameState.PlayerSelection;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}