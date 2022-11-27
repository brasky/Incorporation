using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;

namespace Incorporation
{
    public class SignalRClient : MonoBehaviour
    {
        private static readonly Uri uri = new ("https://localhost:7021/game");
        private static readonly HubConnection gameHub = new (uri, new JsonProtocol(new LitJsonEncoder()));

        public void Connect()
        {
            gameHub.ConnectAsync();
            gameHub.On("ChatMessage", (string arg) => Debug.Log("Chat Message: " + arg));
            
        }
    }
}
