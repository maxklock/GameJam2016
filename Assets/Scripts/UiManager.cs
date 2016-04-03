namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Ui;

    using UnityEngine;

    public class UiManager : MonoBehaviour
    {
        public PlayerUi PlayerUi;

        private readonly Dictionary<PlayerId, PlayerUi> _playerUis = new Dictionary<PlayerId, PlayerUi>();

        public PlayerUi GetPlayerUi(PlayerId id)
        {
            if (!_playerUis.ContainsKey(id))
            {
                _playerUis.Add(id, Instantiate(PlayerUi));
            }

            return _playerUis[id];
        }

        public void ClearPlayerUis()
        {
            _playerUis.Clear();
        }

        #region methods

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