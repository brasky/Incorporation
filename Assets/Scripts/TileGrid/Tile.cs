using System;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class Tile : MonoBehaviour
    {
        private new SpriteRenderer renderer;
        private TileData tileData;
        private GameManager gameManager;

        // Start is called before the first frame update
        void Start()
        {
            
            gameManager = FindObjectOfType<GameManager>();
            tileData = GetComponent<TileData>();
            renderer = GetComponent<SpriteRenderer>();
            renderer.color = GetOwnershipColor();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnMouseOver()
        {
            var prev = renderer.color;
            renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
        }

        private void OnMouseExit()
        {
            renderer.color = GetOwnershipColor();
        }

        private void OnMouseDown()
        {
            if (gameManager.IsPlayerTurn() && tileData.Owner != Ownership.Player)
            {
                ChangeOwnership(Ownership.Player);
                //gameManager.EndTurn();
            }
            Debug.Log($"{tileData.Owner}");
        }

        private void ChangeOwnership(Ownership owner)
        {
            tileData.Owner = owner;
            renderer.color = GetOwnershipColor();
        }

        private Color GetOwnershipColor()
        {
            if (tileData.Owner == Ownership.None)
            {
                return new Color(0.86f, 0.86f, 0.86f);
            }

            if (tileData.Owner == Ownership.Enemy)
            {
                return new Color(0.80f, 0.27f, 0.27f);
            }

            if (tileData.Owner == Ownership.Player)
            {
                return new Color(0.07f, 0.666f, 0);
            }

            throw new ArgumentException($"Tile ownership not set as expected: {tileData.Owner}");
        }
    }
}
