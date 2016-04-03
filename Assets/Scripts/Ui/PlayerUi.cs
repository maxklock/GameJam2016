namespace Assets.Scripts.Ui
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    public class PlayerUi : MonoBehaviour
    {
        #region member vars

        public Text TbxPoints;
        public Text TbxTime;
        public Text TbxMessage;

        const float DefaultMessageTime = 5.0f;

        public Canvas Root;

        private readonly Queue<string> _messages = new Queue<string>();

        private float _messageTimer;

        #endregion

        #region methods

        public void AddMessage(string message)
        {
            _messages.Enqueue(message);
        }

        private void Start()
        {
            _messageTimer = -1;
        }

        private void Update()
        {
            _messageTimer -= Time.deltaTime;
            if (_messageTimer < 0)
            {
                TbxMessage.text = string.Empty;
                _messageTimer = -1;
            }

            if (_messages.Any() && _messageTimer < 0)
            {
                var m = _messages.Dequeue();
                TbxMessage.text = m;
                _messageTimer = DefaultMessageTime;
            }
        }

        #endregion
    }
}