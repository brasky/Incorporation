using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Shared.Players;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class Player
    {
        public string Id { get; set; }

        //Eventually would like a way to lock set to the game manager 
        public PlayerData PlayerData { get; set; }

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        public Player(string id)
        {
            Id = id;
        }
    }
}
