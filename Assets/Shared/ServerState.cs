using Shared.Players;
using Shared.Tiles;
using System;
using System.Collections.Generic;

namespace Shared
{
    public enum GameState
    {
        LOBBY,
        SETUP,
        READYCHECK,
        SETUPCOMPLETE,
        PLAYERTURN,
        MARKETTURN,
        WON,
        LOST
    }

    public enum PlayerAction
    {
        ENDTURN
    }

    [Serializable]
    public class ServerState
    {
        public string Id { get; set; }

        public int MapWidth { get; set; } = 5;
        public int MapHeight { get; set; } = 5;
        public int TurnCount { get; set; } = 0;

        public GameState State { get; set; }

        public PlayerData LocalPlayer { get; set; }

        public bool PlayerActionApproved { get; set; } = false;

#nullable enable
        public PlayerData? ActivePlayer { get; set; }
#nullable disable

        public List<PlayerData> Players { get; set; } = new();

        public List<PlayerData> TurnOrder { get; set; }

        public List<TileData> Tiles { get; set; } = new();

        [NonSerialized]
        public TileData[,] TileMap;

        public ServerState() { }

        public ServerState(string id, PlayerData player)
        {
            Id = id;
            Players.Add(player);
            player.IsHost = true;
            TileMap = new TileData[MapWidth, MapHeight];
        }
    }
}
