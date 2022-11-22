using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incorporation
{
    public class GameManager : MonoBehaviour
    {
        private GameData _gameData;

        [SerializeField]
        private int _numberOfPlayers;

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

        private readonly List<Player> _players = new();

        private int turnCount = -1;
        private bool _haveStartedPollingForRemotePlayer = false;

        void Awake()
        {
            _gameData = ScriptableObject.CreateInstance<GameData>();
            _gameData.State = GameState.SETUP;
            SetupPlayers();
        }

        // Start is called before the first frame update
        void Start()
        {
            _endTurnEventChannel.OnEventRaised += MoveNextPhase;
            _requestGameDataUpdateChannel.OnEventRaised += SendGameData;

            SendGameData();
            MoveNextPhase();
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
        }

        void MoveNextPhase()
        {
            turnCount++;
            _gameData.ActivePlayer = _players[turnCount % _numberOfPlayers];
            if (_gameData.ActivePlayer.IsTheMarket)
            {
                _gameData.State = GameState.MARKETTURN;
            }
            else
            {
                _gameData.State = _gameData.ActivePlayer.IsRemote ? GameState.REMOTEPLAYERTURN : GameState.LOCALPLAYERTURN;
                _haveStartedPollingForRemotePlayer = false;

            }

            _gameDataEventChannel.RaiseEvent(_gameData);
        }

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

            Debug.Log("Still waiting for remote player...");
            
            yield return new WaitForSeconds(1f);

            Debug.Log("Remote player is finally done!");

            MoveNextPhase();
        }
    }
}
