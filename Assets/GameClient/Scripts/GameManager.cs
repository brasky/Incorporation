using Assets.GameClient.ScriptableObjects.EventChannels;
using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.TileGrid;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Incorporation
{
    public class GameManager : MonoBehaviour
    {
        private GameData _gameData;

        [SerializeField]
        private int _numberOfPlayers;

        [SerializeField]
        private PlayerActionEventChannel _actionEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        [SerializeField]
        private VoidEventChannel _requestGameDataUpdateChannel;

        [SerializeField]
        private Player playerPrefab;

        [SerializeField]
        private RemotePlayer remotePlayerPrefab;

        [SerializeField]
        private GridManager gridManager;

        private SignalRClient _client => SignalRClient.Instance;

        private bool _haveStartedPollingForRemotePlayer = false;

        public int TurnOrderIndex { get; private set; } = 0;
        public IReadOnlyCollection<Player> Players => _gameData.Players;

        void Awake()
        {
            _gameData = _gameDataEventChannel.MostRecentState;

            _client.OnServerStateUpdate += UpdateServerState;
            _client.RequestGameState();
        }

        private void UpdateServerState(object _, ServerState serverState)
        {
            Debug.Log($"Received ServerState Update - {serverState.State}");
            _gameData.State = serverState.State;
            _gameData.MapWidth = serverState.MapWidth;
            _gameData.MapHeight = serverState.MapHeight;

            _gameData.PlayerActionApproved = serverState.PlayerActionApproved;

            foreach(var player in _gameData.Players)
            {
                try
                {
                    var newData = serverState.Players.Where(p => p.Id == player.Id).First();

                    player.PlayerData = newData;
                }
                catch(Exception ex)
                {
                    Debug.LogError($"Error updating player {player.Id}");
                }
            }
            try
            {
                _gameData.ActivePlayer = _gameData.Players.Where(p => p.Id == serverState.ActivePlayer.Id).First();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error updating active player");
            }

            gridManager.UpdateGrid(_gameData.Tiles, serverState);
            SendGameData();
        }

        void Start()
        {
            _actionEventChannel.OnEventRaised += ProcessAction;
            _requestGameDataUpdateChannel.OnEventRaised += SendGameData;
        }

        void SendGameData()
        {
            _gameDataEventChannel.RaiseEvent(_gameData);
        }

        void ProcessAction(PlayerAction action)
        {
            _gameData.PlayerAction = action;

            if (_gameData.PlayerAction == PlayerAction.ENDTURN)
            {
                _client.SendAction(action);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_gameData.PlayerAction is not null && _gameData.PlayerActionApproved)
            {
                
            }

            if (_gameData.ActivePlayer is null)
            {
            }

            if (_gameData.State == GameState.PLAYERTURN &&
                _gameData.ActivePlayer is not null &&
                _gameData.ActivePlayer.Id != _client.LocalPlayerId && 
                !_haveStartedPollingForRemotePlayer)
            {
                _haveStartedPollingForRemotePlayer = true;
                StartCoroutine(WaitForRemotePlayer());
            }

            if (_gameData.State == GameState.MARKETTURN)
            {
                //MoveNextPhase();
            }
        }

        IEnumerator WaitForRemotePlayer()
        {
            //Wait for remote player to end their turn
            Debug.Log("Waiting for remote player...");
            
            yield return new WaitForSeconds(1f);

            Debug.Log("Remote player is finally done!");

            //MoveNextPhase();
        }
    }
}
