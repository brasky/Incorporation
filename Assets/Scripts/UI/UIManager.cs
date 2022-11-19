using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.TileGrid;
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
        private TileEventChannel _tileClickChannel = default;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private RectTransform _panel;

        private Button[] _buttons;

        private Tile _currentTile;

        // Start is called before the first frame update
        void Start()
        {
            _panel.gameObject.SetActive(false);
            _tileClickChannel.OnEventRaised += OpenDetailsPanel;
            _gameDataChannel.OnEventRaised += UpdateGameData;
            _buttons = _canvas.GetComponentsInChildren<Button>(includeInactive: true);
        }

        void OnDestroy()
        {
            _tileClickChannel.OnEventRaised -= OpenDetailsPanel;
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
            CloseDetailsPanel();
        }

        private void UpdateEndTurnButton()
        {
            _buttons.Where(b => b.name == "End Turn Button").First().gameObject.SetActive(_gameData.ActivePlayer.IsLocal);
        }

        private void OpenDetailsPanel(Tile tile)
        {
            _currentTile = tile;

            var buyButton = _buttons.Where(b => b.name == "Buy Button").First();
            buyButton.gameObject.SetActive(false);
            if (tile.Owner.IsTheMarket)
                buyButton.gameObject.SetActive(true);

            var text = _panel.GetComponentsInChildren<TextMeshProUGUI>();
            text.Where(t => t.name == "Tile Owner Data").First().text = tile.Owner.name;

            _panel.gameObject.SetActive(true);
        }

        public void CloseDetailsPanel()
        {
            _currentTile = null;
            _panel.gameObject.SetActive(false);
        }

        public void OnBuyButtonPress()
        {

        }
    }
}
