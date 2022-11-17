using Incorporation.Assets.ScriptableObjects.EventChannels;
using System;
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

        // Start is called before the first frame update
        void Start()
        {
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
                    Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }

            //Center the camera on the grid. -10 is the default camera z. 
            camera.transform.position = new Vector3((float)width / 2 - 0.5f, (float)width / 2 - 0.5f, -10);
        }

        private void HandleTileClickEvent(Tile tile)
        {
            Debug.Log(tile.Owner);
        }
    }
}
