﻿using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Shared.Players;
using Shared.Resources;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
        public string Id { get; private set; }

        private GameData _gameData;

        //Eventually would like a way to lock set to the game manager 
        public PlayerData PlayerData { get; set; }

        public Color Color { get; set; } = new Color(0.07f, 0.666f, 0);

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        public bool IsMyTurn => _gameData.ActivePlayer == this;

        public Player(string Id)
        {
            this.Id = Id;
        }

        void Awake()
        {
            Id = SignalRClient.LocalPlayerId;
            PlayerData = GetComponent<PlayerData>();
            _gameDataEventChannel.OnEventRaised += UpdateGameData;
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
            if (price > PlayerData.Money) return false;

            //call server to make purchase
            return true;
        }
        
        public bool TryMakePurchase(Resource resource, int quantity)
        {
            //if (!PlayerData.Resources.TryGetValue(resource, out int available))
            //    return false;

            //if (quantity > available)
            //    return false;

            //call server to make purchase
            return true;
        }

        public bool TryMakePurchase(ResourceCost[] resourceCosts)
        {
            foreach (var resourceCost in resourceCosts)
            {
                //if (!PlayerData.Resources.TryGetValue(resourceCost.Resource, out int available))
                //    return false;

                //if (resourceCost.Quantity > available)
                //    return false;
            }

            foreach(var resourceCost in resourceCosts)
            {
                //_playerData.Resources[resourceCost.Resource] -= resourceCost.Quantity;
                //call server to make purchase
            }

            return true;
        }
    }
}