using Incorporation.Assets.Scripts.Players;
using Shared;
using System.Collections.Generic;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects
{
    public class GameData : ScriptableObject
    {
        public string Id { get; set; }

        public GameState State { get; set; }

        public Player LocalPlayer { get; set; }

#nullable enable
        public Player? ActivePlayer { get; set; }
#nullable disable

        public List<Player> Players { get; set; } = new List<Player>();
    }
}
