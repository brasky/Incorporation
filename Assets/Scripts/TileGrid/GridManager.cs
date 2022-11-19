using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        [SerializeField]
        private TileEventChannel _tileClickEventChannel;

        private Player _theMarket;

        void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            _theMarket = FindObjectsOfType<Player>().Where(p => p.IsTheMarket).First();
            GenerateGrid();

            _tileClickEventChannel.OnEventRaised += HandleTileClickEvent;
        }

        void OnDestroy()
        {
            _tileClickEventChannel.OnEventRaised -= HandleTileClickEvent;
        }

        void GenerateGrid()
        {
            for(int x = 0; x < width; x++)
            {
                for( int y = 0; y < height; y++)
                {
                    var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                    tile.SetOwner(_theMarket);
                }
            }

            //Center the camera on the grid. -10 is the default camera z. 
            camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)width / 2 - 0.5f, -10);
        }

        private void HandleTileClickEvent(Tile tile)
        {
        }
    }
}
