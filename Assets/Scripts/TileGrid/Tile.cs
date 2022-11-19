using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using System;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class Tile : MonoBehaviour
    {
        private new SpriteRenderer renderer;
        private TileData tileData;

        [SerializeField]
        private TileEventChannel _tileClickEventChannel;

        public Player Owner => tileData.Owner;

        // Start is called before the first frame update
        void Awake()
        {
            tileData = GetComponent<TileData>();
            renderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            renderer.color = GetOwnershipColor();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetOwner(Player owner)
        {
            tileData.Owner = owner;
            renderer.color = GetOwnershipColor();
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
            _tileClickEventChannel.RaiseEvent(this);
        }

        private Color GetOwnershipColor()
        {
            if (tileData.Owner.IsTheMarket)
            {
                return new Color(0.86f, 0.86f, 0.86f);
            }

            if (tileData.Owner.IsRemote)
            {
                return new Color(0.80f, 0.27f, 0.27f);
            }

            if (tileData.Owner.IsLocal)
            {
                return new Color(0.07f, 0.666f, 0);
            }

            throw new ArgumentException($"Tile ownership not set as expected: {tileData.Owner}");
        }
    }
}
