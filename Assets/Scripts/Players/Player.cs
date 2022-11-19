using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Players
{
    public class Player : MonoBehaviour
    {
        private GameData _gameData;
        
        public virtual bool IsRemote => false;

        public virtual bool IsLocal => !IsRemote;
        
        public virtual bool IsTheMarket => false;

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        //[SerializeField]
        //private GameStateEventChannel _gameStateEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        void Start()
        {
            //_gameDataEventChannel.OnEventRaised += UpdateGameData;
        }

        void OnDestroy()
        {
            //_gameDataEventChannel.OnEventRaised -= UpdateGameData;
        }

        //void UpdateGameData(GameData newGameData)
        //{
        //    _gameData = newGameData;
        //    IsMyTurn = _gameData.ActivePlayer == this;
        //}

        //public void EndTurn()
        //{
        //    if (IsMyTurn)
        //        _endTurnEventChannel.RaiseEvent();
        //}
    }
}
