using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Resources;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
        private GameData _gameData;

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        public bool IsMyTurn => _gameData.ActivePlayer == this;

        private PlayerData _playerData;

        public virtual Color Color => new Color(0.07f, 0.666f, 0);

        public virtual bool IsRemote => false;

        public virtual bool IsLocal => !IsRemote;
        
        public virtual bool IsTheMarket => false;

        public virtual int StartingMoney => 10;

        public virtual int Income => 0;

        public virtual int Money => _playerData.Money;

        void Awake()
        {
            _playerData = GetComponent<PlayerData>();
            _playerData.Money = StartingMoney;
            _gameDataEventChannel.OnEventRaised += UpdateGameData;
        }

        void OnDestroy()
        {
            _gameDataEventChannel.OnEventRaised -= UpdateGameData;
        }

        public virtual int GetAvailableResourceQuantity(Resource resource) => _playerData.Resources.ContainsKey(resource) ? _playerData.Resources[resource] : 0;
        
        public virtual void AddResource(Resource resource, int quantity)
        {
            if (!_playerData.Resources.TryAdd(resource, quantity))
                _playerData.Resources[resource] += quantity;
        }

        public virtual void ReceiveMoney(int quantity)
        {
            _playerData.Money += quantity;
        }

        public void UpdateGameData(GameData newGameData)
        {
            _gameData = newGameData;
        }

        public bool TryMakePurchase(int price)
        {
            if (price > Money) return false;

            _playerData.Money -= price;
            return true;
        }
        
        public bool TryMakePurchase(Resource resource, int quantity)
        {
            if (!_playerData.Resources.TryGetValue(resource, out int available))
                return false;

            if (quantity > available)
                return false;

            _playerData.Resources[resource] -= quantity;
            return true;
        }

        public bool TryMakePurchase(ResourceCost[] resourceCosts)
        {
            foreach (var resourceCost in resourceCosts)
            {
                if (!_playerData.Resources.TryGetValue(resourceCost.Resource, out int available))
                    return false;

                if (resourceCost.Quantity > available)
                    return false;
            }

            foreach(var resourceCost in resourceCosts)
            {
                _playerData.Resources[resourceCost.Resource] -= resourceCost.Quantity;
            }

            return true;
        }
    }
}
