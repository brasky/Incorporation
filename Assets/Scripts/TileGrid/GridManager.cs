using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private int width;

        [SerializeField]
        private int height;

        [SerializeField]
        private Tile tilePrefab;

        [SerializeField]
        private new Transform camera;

        private Player _theMarket;
        private Tile[] _tiles;

        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            _theMarket = FindObjectsOfType<Player>().Where(p => p.IsTheMarket).First();
            GenerateGrid();
        }

        void OnDestroy()
        {
        }

        void GenerateGrid()
        {
            _tiles = new Tile[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.SetOwner(_theMarket);
                    _tiles[x * width + y] = tile;
                }
            }

            //Center the camera on the grid. -10 is the default camera z. 
            camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)width / 2 - 0.5f, -10);
        }

        public Tile[] GetTilesOwnedByPlayer(Player player, bool includeUnimproved)
        {
            return _tiles.Where(t => t.Owner == player && (includeUnimproved || t.IsImproved)).ToArray();
        }

        public Tile GetRandomTile()
        {
            return _tiles[Random.Range(0, _tiles.Length)];
        }

        public Tile GetRandomUnownedTile()
        {
            var unownedTiles = _tiles.Where(t => t.Owner == _theMarket).ToArray();
            if (unownedTiles.Length == 0)
                return null;

            return unownedTiles[Random.Range(0, unownedTiles.Length)];
        }
    }
}
