using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Resources;
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
        private TileEventChannel _tileClickChannel;

        [SerializeField]
        private VoidEventChannel _requestGameDataUpdateChannel;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private RectTransform _tileDetailsPanel;

        [SerializeField]
        private RectTransform _playerDetailsPanel;

        private Button[] _buttons;

        private Tile _currentTile;

        void Start()
        {
            _tileDetailsPanel.gameObject.SetActive(false);
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
            UpdatePlayerDetailsPanel();
        }

        private void UpdatePlayerDetailsPanel()
        {
            var text = _playerDetailsPanel.GetComponentsInChildren<TextMeshProUGUI>();
            text.Where(t => t.name == "Money Data").First().text = _gameData.LocalPlayer.Money.ToString();
            text.Where(t => t.name == "Wood Data").First().text = _gameData.LocalPlayer.GetAvailableResourceQuantity(Resource.Wood).ToString();
            text.Where(t => t.name == "Oil Data").First().text = _gameData.LocalPlayer.GetAvailableResourceQuantity(Resource.Oil).ToString();
        }

        private void UpdateEndTurnButton()
        {
            _buttons.Where(b => b.name == "End Turn Button").First().gameObject.SetActive(_gameData.ActivePlayer.IsLocal);
        }

        private void SetBuyButtonVisibility()
        {
            var buyButton = _buttons.Where(b => b.name == "Buy Button").First();
            buyButton.gameObject.SetActive(false);
            if (_currentTile.Owner.IsTheMarket)
                buyButton.gameObject.SetActive(true);
        }

        private void OpenDetailsPanel(Tile tile)
        {
            _currentTile = tile;

            SetBuyButtonVisibility();

            var text = _tileDetailsPanel.GetComponentsInChildren<TextMeshProUGUI>();
            text.Where(t => t.name == "Tile Owner Data").First().text = tile.Owner.name;

            text.Where(t => t.name == "Tile Resource Data").First().text = tile.Resources[0].ToString();

            text.Where(t => t.name == "Tile Yield Data").First().text = tile.Yield.ToString();

            _tileDetailsPanel.gameObject.SetActive(true);
        }

        public void CloseDetailsPanel()
        {
            _currentTile = null;
            _tileDetailsPanel.gameObject.SetActive(false);
        }

        public void OnBuyButtonPress()
        {
            if (_gameData.ActivePlayer.TryMakePurchase(_currentTile.Price))
            {
                _currentTile.SetOwner(_gameData.ActivePlayer);
                _requestGameDataUpdateChannel.RaiseEvent();
            }

            SetBuyButtonVisibility();
            OpenDetailsPanel(_currentTile);
        }
    }
}
