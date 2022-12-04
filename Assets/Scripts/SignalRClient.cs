using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using Incorporation.Assets.ScriptableObjects.EventChannels;

namespace Incorporation
{
    public static class SignalRClient
    {
        private static readonly Uri uri = new ("https://localhost:7021/game");
        public static readonly HubConnection hub = new (uri, new JsonProtocol(new LitJsonEncoder()));
        public static string LobbyId { get; set; }
        public static ServerState serverState;

        static SignalRClient()
        {
            hub.On<ServerState>("RefreshGameState", (serverState) =>
            {
                Debug.Log("Refreshing Game State...");
                Debug.Log(serverState.ToString());
                SignalRClient.serverState = serverState;
            });
        }

        public static void Connect()
        {
            if (hub.State == ConnectionStates.Initial || hub.State == ConnectionStates.Redirected)
            {
                Debug.Log("Connecting...");
                hub.ConnectAsync();
            }
        }

        public static void CreateLobby()
        {
            Debug.Log("Creating Lobby...");
            hub.Send("CreateLobby");
        }
    }
}
