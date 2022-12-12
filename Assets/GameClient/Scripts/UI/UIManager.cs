using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.TileGrid;
using Shared;
using Shared.Resources;
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
            if (_gameData.ActivePlayer.Id == SignalRClient.LocalPlayerId)
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
            text.Where(t => t.name == "Money Data").First().text = _gameData.LocalPlayer.PlayerData.Money.ToString();
            //text.Where(t => t.name == "Wood Data").First().text = _gameData.LocalPlayer.PlayerData.GetAvailableResourceQuantity(Resource.Wood).ToString();
            //text.Where(t => t.name == "Oil Data").First().text = _gameData.LocalPlayer.PlayerData.GetAvailableResourceQuantity(Resource.Oil).ToString();
        }

        private void UpdateEndTurnButton()
        {
            _buttons.Where(b => b.name == "End Turn Button").First().gameObject.SetActive(_gameData.ActivePlayer.Id == SignalRClient.LocalPlayerId);
        }

        private void SetBuyButtonVisibility()
        {
            var buyButton = _buttons.Where(b => b.name == "Buy Button").First();
            buyButton.gameObject.SetActive(false);
            if (_currentTile.Owner is null)
                buyButton.gameObject.SetActive(true);
        }
        
        private void SetImproveButtonVisibility()
        {
            var improveButton = _buttons.Where(b => b.name == "Improve Button").First();
            improveButton.gameObject.SetActive(_gameData.LocalPlayer == _currentTile.Owner && !_currentTile.IsImproved);
        }

        private void OpenDetailsPanel(Tile tile)
        {
            CloseDetailsPanel();

            _currentTile = tile;
            _currentTile.SetDetailsPaneSelected();
            SetBuyButtonVisibility();
            SetImproveButtonVisibility();

            var text = _tileDetailsPanel.GetComponentsInChildren<TextMeshProUGUI>();
            text.Where(t => t.name == "Tile Owner Data").First().text = tile.Owner.name;

            text.Where(t => t.name == "Tile Resource Data").First().text = tile.Resources[0].ToString();

            text.Where(t => t.name == "Tile Yield Data").First().text = tile.Yield.ToString();

            text.Where(t => t.name == "Tile IsImproved Data").First().text = tile.IsImproved.ToString();

            text.Where(t => t.name == "Tile ResourceCost Data").First().text = tile.GetResourceCostsAsString();

            _tileDetailsPanel.gameObject.SetActive(true);
        }

        public void CloseDetailsPanel()
        {
            if (_currentTile == null)
                return;

            _currentTile.SetDetailsPaneDeselected();
            _tileDetailsPanel.gameObject.SetActive(false);
            _currentTile = null;
        }

        public void OnBuyButtonPress()
        {
            if (_gameData.ActivePlayer.TryMakePurchase(_currentTile.Price))
            {
                _currentTile.SetOwner(_gameData.ActivePlayer);
                _requestGameDataUpdateChannel.RaiseEvent();
            }

            SetBuyButtonVisibility();
            SetImproveButtonVisibility();
            OpenDetailsPanel(_currentTile);
        }

        public void OnImproveButtonPress()
        {
            if (_gameData.ActivePlayer.TryMakePurchase(_currentTile.ResourceCosts))
            {
                _currentTile.Improve();
                SetImproveButtonVisibility();
                OpenDetailsPanel(_currentTile);
                UpdatePlayerDetailsPanel();
            }
        }
    }
}
