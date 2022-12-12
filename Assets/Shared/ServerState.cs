using Shared.Players;
using System;
using System.Collections.Generic;

namespace Shared
{
    public enum GameState
    {
        LOBBY,
        SETUP,
        LOCALPLAYERTURN,
        REMOTEPLAYERTURN,
        MARKETTURN,
        WON,
        LOST
    }

    [Serializable]
    public class ServerState
    {
        public string Id { get; set; }

        public GameState State { get; set; }

        public PlayerData LocalPlayer { get; set; }

#nullable enable
        public PlayerData? ActivePlayer { get; set; }
#nullable disable

        public List<PlayerData> Players { get; set; } = new();

        public ServerState() { }
        public ServerState(string id)
        {
            Id = id;
        }
        public ServerState(string id, PlayerData player)
        {
            Id = id;
            Players.Add(player);
        }
    }
}
