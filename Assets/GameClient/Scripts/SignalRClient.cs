using System;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using Shared;
using System.Linq;

namespace Incorporation
{
    public class SignalRClient
    {
        private static SignalRClient _instance;
        public static SignalRClient Instance
        {
            get
            {
                return _instance is null ? _instance = new SignalRClient() : _instance;
            }
        }

        private static readonly Uri uri = new("https://localhost:7021/game");
        public HubConnection Hub { get; private set;} = new (uri, new JsonProtocol(new LitJsonEncoder()));
        public string LocalPlayerId { get; private set; } = string.Empty;
        private ServerState ServerState;
        public event EventHandler<ServerState> OnServerStateUpdate;

        SignalRClient()
        {
            Hub.OnClosed += (_) => _instance = null;
            Hub.OnConnected += (_) => SetChannels();

            _instance = this;
        }

        private void SetChannels()
        {
            Debug.Log("Connected! Setting up channels.");

            Hub.On<string>("JoinedGame", (localPlayerId) =>
            {
                Debug.Log($"Joined game as {localPlayerId}");
                LocalPlayerId = localPlayerId;
                RequestGameState();
            });

            Hub.On<ServerState>("RefreshGameState", (serverState) =>
            {
                Debug.Log("Refreshing Game State...");
                ServerState = serverState;
                ServerState.LocalPlayer = ServerState.Players.Where(p => p.Id == LocalPlayerId).First();
                OnServerStateUpdate?.Invoke(null, ServerState);
            });

            Hub.On<ServerState>("NewLobbyCreated", (serverState) =>
            {
                Debug.Log("New Lobby Created...");
                Debug.Log(serverState.Id);
                ServerState = serverState;
                LocalPlayerId = serverState.Players[0].Id;
                OnServerStateUpdate?.Invoke(null, ServerState);
            });
        }

        public void Connect()
        {
            if (Hub.State != ConnectionStates.Connected)
            {
                
                Debug.Log("Connecting...");
                Hub.StartConnect();
            }
        }

        public void CreateLobby()
        {
            Debug.Log("Creating Lobby...");
            Hub.Send("CreateLobby");
        }

        public void RequestGameState()
        {
            Hub.Send("RequestGameState");
        }

        public void JoinGame(string id)
        {
            Hub.Send("JoinGame", id);
        }

        public void Disconnect()
        {
            Hub.StartClose();
            _instance = null;
        }

        public void Ready()
        {
            Hub.Send("Ready", ServerState.Id);
        }

        public void StartGame()
        {
            Hub.Send("StartGame", ServerState.Id);
        }

        public void SendAction(PlayerAction actionTaken)
        {
            Hub.Send("Action", actionTaken);
        }
    }
}
