using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Incorporation
{
    public class UIManager : MonoBehaviour
    {
        private GameData _gameData;

        [SerializeField]
        private GameDataEventChannel _gameDataChannel;

        [SerializeField]
        private VoidEventChannel _endTurnChannel;

        [SerializeField]
        private Canvas _canvas;

        private Button[] _buttons;

        // Start is called before the first frame update
        void Start()
        {
            _gameDataChannel.OnEventRaised += UpdateGameData;
            _buttons = _canvas.GetComponentsInChildren<Button>();
        }

        void OnDestroy()
        {
            _gameDataChannel.OnEventRaised -= UpdateGameData;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void EndTurnButtonPress()
        {
            if (_gameData.ActivePlayer.IsLocal)
                _endTurnChannel.RaiseEvent();
        }

        private void UpdateGameData(GameData gameData)
        {
            _gameData = gameData;

            if (gameData.State == GameState.SETUP)
                return;

            UpdateEndTurnButton();
        }

        private void UpdateEndTurnButton()
        {
            _buttons.Where(b => b.name == "End Turn Button").First().gameObject.SetActive(_gameData.ActivePlayer.IsLocal);
        }
    }
}
