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
    private const float _refreshTime = 5.0f;

    public void Back()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void UpdateGameData(GameData gameData)
    {
        _gameData = gameData;

        if (gameData.State == GameState.SETUP)
            return;
    }

    void Awake()
    {
        SignalRClient.OnServerStateUpdate += UpdateServerState;
        _gameData = ScriptableObject.CreateInstance<GameData>();
        _gameData.State = GameState.LOBBY;
    }

    private void UpdateServerState(object _, ServerState serverState)
    {
        Debug.Log("Received ServerState Update");
        _gameData.State = serverState.State;
        _gameData.Players.Clear();
        foreach (var player in serverState.Players)
        {
            if (player.Id == SignalRClient.LocalPlayerId)
            {
                var localPlayer = new Player(player.Id);
                _gameData.Players.Add(localPlayer);
                _gameData.LocalPlayer = localPlayer;
            }
            else
            {
                _gameData.Players.Add(new RemotePlayer(player.Id));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameDataEventChannel.OnEventRaised += UpdateGameData;
    }

    void OnDestroy()
    {
        _gameDataEventChannel.OnEventRaised -= UpdateGameData;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > _refreshTime)
        {
            _timer = 0.0f;

            SignalRClient.RequestGameState();

            _textBoxes = FindObjectsOfType<TextMeshProUGUI>();
            var lobbyId = _textBoxes.Where(t => t.name == "LobbyIDText").First();
            lobbyId.text = $"Lobby ID: {SignalRClient.LobbyId}";
            UpdatePlayerList();
        }
    }

    private void UpdatePlayerList()
    {
        if (_gameData.Players is null) return;

        for (var i = 0; i < _gameData.Players.Count; i++)
        {
            var playerText = FindObjectsOfType<TextMeshProUGUI>().Where(t => t.name == $"Player{i + 1}").First();
            playerText.text = _gameData.Players[i].Id;
        }
    }
}
