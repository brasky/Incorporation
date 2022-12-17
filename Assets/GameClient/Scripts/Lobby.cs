using Incorporation;
using Incorporation.Assets.ScriptableObjects;
using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Shared;
using System.Linq;
using TMPro;
using UnityEngine;
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
        SceneManager.LoadScene("GameScene");
    }

    void Awake()
    {

    }

    void Start()
    {
        _client.OnServerStateUpdate += UpdateServerState;
        _client.CreateLobby();
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

        Debug.Log("Received ServerState Update");
        _gameData.Id = serverState.Id;
        _gameData.State = serverState.State;
        _gameData.Players.Clear();
        foreach (var player in serverState.Players)
        {
            if (player.Id == _client.LocalPlayerId)
            {
                var localPlayer = Instantiate(playerPrefab);
                localPlayer.Id = player.Id;
                DontDestroyOnLoad(localPlayer);
                _gameData.Players.Add(localPlayer);
                _gameData.LocalPlayer = localPlayer;
            }
            else
            {
                var remotePlayer = Instantiate(remotePlayerPrefab);
                remotePlayer.Id = player.Id;
                DontDestroyOnLoad(remotePlayer);
                _gameData.Players.Add(remotePlayer);
            }
        }
        UpdateLobbyId();
        UpdatePlayerList();
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

            UpdateLobbyId();
            UpdatePlayerList();
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
