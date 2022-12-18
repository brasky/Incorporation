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

        public int MapWidth { get; set; } = 5;
        public int MapHeight { get; set; } = 5;

        public GameState State { get; set; }

        public PlayerData LocalPlayer { get; set; }

#nullable enable
        public PlayerData? ActivePlayer { get; set; }
#nullable disable

        public List<PlayerData> Players { get; set; } = new();
        
        public List<TileData> Tiles { get; set; } = new();

        [NonSerialized]
        public TileData[,] TileMap;

        public ServerState() { }

        public ServerState(string id, PlayerData player)
        {
            Id = id;
            Players.Add(player);
            TileMap = new TileData[MapWidth, MapHeight];
        }
    }
}
