using Incorporation.Assets.Scripts.Players;
using Shared;
using Shared.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class GridManager : MonoBehaviour
    {
        private int width;

        private int height;

        [SerializeField]
        private Tile tilePrefab;

        [SerializeField]
        private new Transform camera;

        private Player _theMarket;
        private List<Tile> _tiles;
        private Tile[,] _tileMap;

        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        void OnDestroy()
        {
        }

        public void UpdateGrid(List<Tile> gameDataTiles, ServerState serverState)
        {
            try
            {
                width = serverState.MapWidth;
                height = serverState.MapHeight;

                if (_tileMap is null || _tiles is null)
                {
                    GenerateGrid(serverState.Tiles);
                    gameDataTiles.AddRange(_tiles);
                }
            }
            catch(Exception ex)
            {
                Debug.LogError($"Exception during UpdateGrid");
                Debug.LogError(ex.Message);
            }
        }

        public void GenerateGrid(List<TileData> tiles)
        {
            _tileMap = new Tile[width, height];
            _tiles = new List<Tile>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile._tileData = tiles[x * width + y];
                    _tileMap[x, y] = tile;
                    _tiles.Add(tile);
                }
            }

            //Center the camera on the grid. -10 is the default camera z. 
            camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)width / 2 - 0.5f, -10);
        }

        public Tile[] GetTilesOwnedByPlayer(Player player, bool includeUnimproved)
        {
            return _tiles.Where(t => t.Owner.Id == player.Id && (includeUnimproved || t.IsImproved)).ToArray();
        }

        public Tile GetRandomTile()
        {
            return _tiles[0];
            //return _tileMap[Random.Range(0, _tiles.Length)];
        }

        public Tile GetRandomUnownedTile()
        {
            return _tiles[0];
            //var unownedTiles = _tiles.Where(t => t.Owner == null).ToArray();
            //if (unownedTiles.Length == 0)
            //    return null;

            //return unownedTiles[Random.Range(0, unownedTiles.Length)];
        }
    }
}
