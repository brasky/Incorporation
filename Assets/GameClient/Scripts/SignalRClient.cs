using System;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using Shared;
using System.Linq;

namespace Incorporation
{
    public static class SignalRClient
    {
        private static readonly Uri uri = new ("https://localhost:7021/game");
        public static readonly HubConnection Hub = new (uri, new JsonProtocol(new LitJsonEncoder()));
        public static string LocalPlayerId { get; private set; } = string.Empty;
        private static ServerState ServerState;
        public static event EventHandler<ServerState> OnServerStateUpdate;

        static SignalRClient()
        {
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

        public static void Connect()
        {
            if (Hub.State == ConnectionStates.Initial || Hub.State == ConnectionStates.Redirected)
            {
                Debug.Log("Connecting...");
                Hub.ConnectAsync();
            }
        }

        public static void CreateLobby()
        {
            Debug.Log("Creating Lobby...");
            Hub.Send("CreateLobby");
        }

        public static void RequestGameState()
        {
            Hub.Send("RequestGameState");
        }

        public static void JoinGame(string id)
        {
            Hub.Send("JoinGame", id);
        }

        public static void Disconnect()
        {
            Hub.CloseAsync();
        }

        internal static void StartGame()
        {
            Hub.Send("StartGame", ServerState.Id);
        }
    }
}
