namespace Assets.Scripts
{
    using System;
    using System.Linq;

    using Assets.Scripts.Ui;

    using UnityEditor;

    using UnityEngine;
    
    public class Game : MonoBehaviour
    {
        #region member vars

        public bool AllowEmptyViewPort = true;
        public InputType[] InputTypes = { InputType.Joystick1, InputType.Joystick2, InputType.Joystick3, InputType.Keyboard };
        public Player Player;
        public int PlayerCount = 2;
        public Orientation SplitScreen = Orientation.Horizontal;

        public Vector3[] StartPositions = { new Vector3(-60, 10, -60), new Vector3(60, 10, 60), new Vector3(60, 10, -60), new Vector3(-60, 10, 60) };
        
        public float StartTime;

        public PlayerUi PlayerUi;

        private Player[] _players;

        #endregion

        #region methods

        public void AddMessage(string message)
        {
            AddMessage(message, PlayerId.None);
        }

        public void AddMessage(string message, PlayerId id)
        {
            if (id != PlayerId.None)
            {
                _players.Single(p => p.Id == id).AddMessage(message);
                return;
            }

            foreach (var player in _players)
            {
                player.AddMessage(message);
            }
        }

        public void InitPlayers()
        {
            var players = GetComponentsInChildren<Player>();
            foreach (var player in players)
            {
                DestroyImmediate(player.gameObject);
            }

            for (var i = 0; i < PlayerCount; i++)
            {
                var player = Instantiate(Player);
                player.name += " " + (i + 1);
                player.transform.parent = transform;
                player.Id = (PlayerId)(i + 1);
                player.InputType = InputTypes[i];
                player.PlayerUi = Instantiate(PlayerUi);
                player.Game = this;

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
                player.UpdateTime();
            }

            Start();
        }

        // Use this for initialization
        private void Start()
        {
            StartTime = Time.time;
            _players = FindObjectsOfType<Player>();
            AddMessage("Start");
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}