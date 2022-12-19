using Incorporation;
using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Shared;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    [SerializeField]
    private GameDataEventChannel _gameDataEventChannel;

    private GameData _gameData;
    private TextMeshProUGUI[] _textBoxes;

    private float _timer = 0.0f;
    private const float _refreshTime = 10.0f;

    [SerializeField]
    private Player playerPrefab;

    [SerializeField]
    private RemotePlayer remotePlayerPrefab;

    private SignalRClient _client => SignalRClient.Instance;

    public void Back()
    {
        _client.Disconnect();
        SceneManager.LoadScene("MenuScene");
    }

    public void CopyLobbyId()
    {
        GUIUtility.systemCopyBuffer = _gameData.Id;
    }

    public void StartGame()
    {
        _client.StartGame();
    }

    void Awake()
    {

    }

    void Start()
    {
        _client.OnServerStateUpdate += UpdateServerState;
    }

    void OnDestroy()
    {
        _client.OnServerStateUpdate -= UpdateServerState;
    }

    private void UpdateServerState(object _, ServerState serverState)
    {
        if (_gameData is null)
        {
            _gameData = ScriptableObject.CreateInstance<GameData>();
            _gameData.State = GameState.LOBBY;
        }

        Debug.Log($"LOBBY: Received ServerState Update - {serverState.State}");
        _gameData.Id = serverState.Id;
        _gameData.State = serverState.State;
        _gameData.Players.Clear();
        foreach (var player in serverState.Players)
        {
            if (player.Id == _client.LocalPlayerId)
            {
                var localPlayer = new Player(player.Id);
                localPlayer.PlayerData = player;
                localPlayer.PlayerData.IsReady = player.IsReady;
                _gameData.Players.Add(localPlayer);
                _gameData.LocalPlayer = localPlayer;
            }
            else
            {
                var remotePlayer = new RemotePlayer(player.Id);
                remotePlayer.Id = player.Id;
                remotePlayer.PlayerData = player;
                remotePlayer.PlayerData.IsReady = player.IsReady;
                _gameData.Players.Add(remotePlayer);
            }
        }
        UpdateLobbyId();
        UpdatePlayerList();

        if (_gameData.State == GameState.SETUPCOMPLETE)
        {
            _gameDataEventChannel.RaiseEvent(_gameData);
            SceneManager.LoadScene("GameScene");
        }

        _gameDataEventChannel.RaiseEvent(_gameData);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _refreshTime)
        {
            _timer = 0.0f;
            _client.RequestGameState();
        }

        if (_gameData is not null && _gameData.State == GameState.READYCHECK && !_gameData.LocalPlayer.PlayerData.IsReady)
        {
            _gameData.LocalPlayer.PlayerData.IsReady = true;
            _client.Ready();
        }
    }

    private void UpdateLobbyId()
    {
        _textBoxes = FindObjectsOfType<TextMeshProUGUI>();
        var lobbyId = _textBoxes.Where(t => t.name == "LobbyIDText").First();
        lobbyId.text = $"Lobby ID: {_gameData.Id}";
    }

    private void UpdatePlayerList()
    {
        for (var i = 0; i < _gameData.Players.Count; i++)
        {
            var playerText = FindObjectsOfType<TextMeshProUGUI>().Where(t => t.name == $"Player{i + 1}").First();
            playerText.text = _gameData.Players[i].Id;
        }
    }
}
