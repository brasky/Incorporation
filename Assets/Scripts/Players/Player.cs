using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Resources;
using System;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
        private GameData _gameData;

        public bool IsMyTurn => _gameData.ActivePlayer == this;

        private PlayerData _playerData;
        
        public virtual bool IsRemote => false;

        public virtual bool IsLocal => !IsRemote;
        
        public virtual bool IsTheMarket => false;

        public virtual int StartingMoney => 10;

        public virtual int Money => _playerData.Money;
        public virtual int GetAvailableResourceQuantity(Resource resource) => _playerData.Resources.ContainsKey(resource) ? _playerData.Resources[resource] : 0;

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            _playerData.Money = StartingMoney;
            _gameDataEventChannel.OnEventRaised += UpdateGameData;
        }

        void Start()
        {
        }

        void OnDestroy()
        {
            _gameDataEventChannel.OnEventRaised -= UpdateGameData;
        }

        public void UpdateGameData(GameData newGameData)
        {
            _gameData = newGameData;
        }

        public bool TryMakePurchase(int price)
        {
            if (price > Money || !IsMyTurn) return false;

            _playerData.Money -= price;
            return true;
        }
    }
}
