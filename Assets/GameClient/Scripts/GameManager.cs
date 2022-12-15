using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.TileGrid;
using Shared;
using Shared.Resources;
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
        private int _baseIncome;

        [SerializeField]
        private VoidEventChannel _endTurnEventChannel;

        [SerializeField]
        private GameDataEventChannel _gameDataEventChannel;

        [SerializeField]
        private VoidEventChannel _requestGameDataUpdateChannel;

        [SerializeField]
        private Player playerPrefab;

        //[SerializeField]
        //private Player theMarketPrefab;

        [SerializeField]
        private RemotePlayer remotePlayerPrefab;

        [SerializeField]
        private GridManager gridManager;

        private int turnCount = -1;
        private bool _haveStartedPollingForRemotePlayer = false;

        public int TurnOrderIndex { get; private set; } = 0;
        public IReadOnlyList<Player> TurnOrder {  get; private set; }

        void Awake()
        {
            _gameData = _gameDataEventChannel.MostRecentState;

            //TODO: shuffle turn order
            TurnOrder = _gameData.Players;

            SignalRClient.OnServerStateUpdate += UpdateServerState;
            SignalRClient.StartGame();
            //SetupPlayers();
        }

        private void UpdateServerState(object _, ServerState serverState)
        {
            Debug.Log("Received ServerState Update");
            _gameData.State = serverState.State;
            foreach(var player in _gameData.Players)
            {
                var newData = serverState.Players.Where(p => p.Id == player.Id).First();

                player.PlayerData = newData;
            }

            SendGameData();
        }

        void Start()
        {
            _endTurnEventChannel.OnEventRaised += MoveNextPhase;
            _requestGameDataUpdateChannel.OnEventRaised += SendGameData;
            //MoveNextPhase();
        }

        private void GivePlayersStartingTiles()
        {
            foreach(var player in _gameData.Players)
            {
                var tile = gridManager.GetRandomUnownedTile();
                tile.SetOwner(player);
            }
        }

        void SendGameData()
        {
            _gameDataEventChannel.RaiseEvent(_gameData);
        }

        void MoveNextPhase()
        {
            var previousState = _gameData.State;
            Debug.Log($"LEAVING PHASE {_gameData.State}");
            turnCount++;
            _gameData.ActivePlayer = TurnOrder[turnCount % _gameData.Players.Count];

            if (previousState == GameState.SETUP)
            {
                _gameData.State = _gameData.ActivePlayer.Id == SignalRClient.LocalPlayerId ? GameState.LOCALPLAYERTURN 
                                                                                            : GameState.REMOTEPLAYERTURN;
            }

            if(previousState == GameState.LOCALPLAYERTURN)
            {
                _gameData.State = GameState.REMOTEPLAYERTURN;
                _haveStartedPollingForRemotePlayer = false;
            }
            
            if(previousState == GameState.REMOTEPLAYERTURN)
            {
                _gameData.State = GameState.LOCALPLAYERTURN;
            }

            Debug.Log($"ENTERING PHASE {_gameData.State}");


            //_gameData.ActivePlayer = _players[turnCount % _numberOfPlayers];
            //if (_gameData.ActivePlayer.IsTheMarket)
            //{
            //    _gameData.State = GameState.MARKETTURN;
            //}
            //else
            //{
            //    _gameData.State = _gameData.ActivePlayer.IsRemote ? GameState.REMOTEPLAYERTURN : GameState.LOCALPLAYERTURN;
            //    _haveStartedPollingForRemotePlayer = false;
            //}

            //if (_gameData.State == GameState.LOCALPLAYERTURN)
            //{
            //    GiveIncomeToActivePlayer();
            //    GiveTileYieldsToActivePlayer();
            //}

            //_gameDataEventChannel.RaiseEvent(_gameData);
        }

        //private void GiveIncomeToActivePlayer()
        //{
        //    _gameData.ActivePlayer.ReceiveMoney(_gameData.ActivePlayer.Income + _baseIncome);
        //}

        //private void GiveTileYieldsToActivePlayer()
        //{
        //    var ownedTiles = gridManager.GetTilesOwnedByPlayer(_gameData.ActivePlayer, includeUnimproved: true);

        //    foreach(Resource resource in Enum.GetValues(typeof(Resource)))
        //    {
        //        var improvedResourceTiles = ownedTiles.Where(t => t.IsImproved && t.Resources.Any(r => r == resource)).ToArray();
        //        var unimprovedResourceTiles = ownedTiles.Where(t => !t.IsImproved && t.Resources.Any(r => r == resource)).ToArray();
        //        _gameData.ActivePlayer.AddResource(resource, (int)(improvedResourceTiles.Sum(t => Math.Ceiling(t.Yield * 1.5)) + unimprovedResourceTiles.Sum(t => t.Yield)));
        //    }
        //}

        // Update is called once per frame
        void Update()
        {
            if (_gameData.State == GameState.SETUP)
            {
                GivePlayersStartingTiles();
                MoveNextPhase();
                SendGameData();
            }

            if (_gameData.State == GameState.REMOTEPLAYERTURN && !_haveStartedPollingForRemotePlayer)
            {
                _haveStartedPollingForRemotePlayer = true;
                StartCoroutine(WaitForRemotePlayer());
            }

            if (_gameData.State == GameState.MARKETTURN)
            {
                MoveNextPhase();
            }
        }

        IEnumerator WaitForRemotePlayer()
        {
            //Wait for remote player to end their turn
            Debug.Log("Waiting for remote player...");
            
            yield return new WaitForSeconds(1f);

            Debug.Log("Remote player is finally done!");

            MoveNextPhase();
        }
    }
}
