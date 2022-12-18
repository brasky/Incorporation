using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.TileGrid;
using Shared;
using System.Collections.Generic;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects
{
    public class GameData : ScriptableObject
    {
        public string Id { get; set; }

        public int MapWidth { get; set; }

        public int MapHeight { get; set; }

        public GameState State { get; set; }

        public Player LocalPlayer { get; set; }

#nullable enable
        public Player? ActivePlayer { get; set; }
#nullable disable

        public List<Player> Players { get; set; } = new List<Player>();

        public List<Tile> Tiles { get; set; } = new List<Tile>();

        public Tile[,] TileMap { get; set; }
    }
}
