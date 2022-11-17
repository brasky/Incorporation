using System.Collections;
using System.Collections.Generic;
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

        // Start is called before the first frame update
        void Start()
        {
            GenerateGrid();
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
    }
}
