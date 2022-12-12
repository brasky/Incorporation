using UnityEngine;
using Incorporation.Assets.Scripts.Players;
using Shared.Resources;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class TileData : MonoBehaviour
    {
        public int Price => 1;

        public Player Owner;

        public Resource[] Resources;
    }
}
