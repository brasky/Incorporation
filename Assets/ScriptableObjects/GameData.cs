using Incorporation.Assets.Scripts.Player;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects
{
    public enum GameState
    {
        SETUP,
        LOCALPLAYERTURN,
        REMOTEPLAYERTURN,
        WON,
        LOST
    }

    public class GameData : ScriptableObject
    {
        public GameState State { get; set; }

#nullable enable
        public Player? ActivePlayer { get; set; }
#nullable disable
    }
}
