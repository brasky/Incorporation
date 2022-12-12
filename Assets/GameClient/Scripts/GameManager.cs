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

        [SerializeField]
        private Player theMarketPrefab;

        [SerializeField]
        private RemotePlayer remotePlayerPrefab;

        [SerializeField]
        private GridManager gridManager;

        private readonly List<Player> _players = new();

        private int turnCount = -1;
        private bool _haveStartedPollingForRemotePlayer = false;

        void Awake()
        {
            SignalRClient.OnServerStateUpdate += UpdateServerState;
            _gameData = ScriptableObject.CreateInstance<GameData>();
            _gameData.State = GameState.SETUP;
            SetupPlayers();
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

            //GivePlayersStartingTiles();

            SendGameData();
            //MoveNextPhase();
        }

        private void GivePlayersStartingTiles()
        {
            foreach(var player in _players)
            {
                var tile = gridManager.GetRandomUnownedTile();
                tile.SetOwner(player);
            }
        }

        void SendGameData()
        {
            _gameDataEventChannel.RaiseEvent(_gameData);
        }

        void SetupPlayers()
        {
            if (_numberOfPlayers == 0)
            {
                Debug.LogError("The game requires at least 1 player to be set in the Game Manager");
            }

            var local = Instantiate(playerPrefab);
            local.name = "Local Player";
            _players.Add(local);

            _gameData.LocalPlayer = local;

            var theMarket = Instantiate(theMarketPrefab);
            theMarket.name = "The Market";

            for (int i = 1; i < _numberOfPlayers; i++)
            {
                var remote = Instantiate(remotePlayerPrefab);
                remote.name = $"Remote Player {i}";
                _players.Add(remote);
            }

            _players.Add(theMarket);

            _gameData.Players = _players;
        }

        void MoveNextPhase()
        {
            //turnCount++;
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

            _gameDataEventChannel.RaiseEvent(_gameData);
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
