using System;
using UnityEngine;

namespace Incorporation
{
    public class Tile : MonoBehaviour
    {
        public enum Ownership
        {
            None,
            Player,
            Enemy
        };

        public Ownership Owner = Ownership.None;

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
            if (gameManager.IsPlayerTurn() && Owner != Ownership.Player)
            {
                ChangeOwnership(Ownership.Player);
                gameManager.EndTurn();
            }
            Debug.Log($"{Owner}");
        }

        private void ChangeOwnership(Ownership owner)
        {
            Owner = owner;
            renderer.color = GetOwnershipColor();
        }
        
        private Color GetOwnershipColor()
        {
            if (Owner == Ownership.None)
            {
                return new Color(0.86f, 0.86f, 0.86f);
            }

            if (Owner == Ownership.Enemy)
            {
                return new Color(0.80f, 0.27f, 0.27f);
            }

            if (Owner == Ownership.Player)
            {
                return new Color(0.07f, 0.666f, 0);
            }

            throw new ArgumentException($"Tile ownership not set as expected: {Owner}");
        }
    }
}
