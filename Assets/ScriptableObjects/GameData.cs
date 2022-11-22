using Incorporation.Assets.Scripts.Players;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects
{
    public enum GameState
    {
        SETUP,
        LOCALPLAYERTURN,
        REMOTEPLAYERTURN,
        MARKETTURN,
        WON,
        LOST
    }

    public class GameData : ScriptableObject
    {
        public GameState State { get; set; }

        public Player LocalPlayer { get; set; }

#nullable enable
        public Player? ActivePlayer { get; set; }
#nullable disable
    }
}
