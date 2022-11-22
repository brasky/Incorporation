using UnityEngine;
using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.Resources;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class TileData : MonoBehaviour
    {
        public int Price => 1;

        public Player Owner;

        public Resource[] Resources;
    }
}
