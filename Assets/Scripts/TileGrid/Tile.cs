using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.Resources;
using System;
using TMPro;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private TileData _tileData;

        [SerializeField]
        private TileEventChannel _tileClickEventChannel;

        public Player Owner => _tileData.Owner;
        public Resource[] Resources => _tileData.Resources;
        public int Price => _tileData.Price;

        // Start is called before the first frame update
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _tileData = GetComponent<TileData>();

            var randomResource = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Resource)).Length);
            _tileData.Resources = new Resource[1] { (Resource)randomResource };

            var text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = Resources[0].ToString();
        }

        void Start()
        {
            _renderer.color = GetOwnershipColor();
        }

        public void SetOwner(Player owner)
        {
            _tileData.Owner = owner;
            _renderer.color = GetOwnershipColor();
        }

        private void OnMouseOver()
        {
            var prev = _renderer.color;
            _renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
        }

        private void OnMouseExit()
        {
            _renderer.color = GetOwnershipColor();
        }

        private void OnMouseDown()
        {
            _tileClickEventChannel.RaiseEvent(this);
        }

        private Color GetOwnershipColor()
        {
            if (_tileData.Owner.IsTheMarket)
            {
                return new Color(0.86f, 0.86f, 0.86f);
            }

            if (_tileData.Owner.IsRemote)
            {
                return new Color(0.80f, 0.27f, 0.27f);
            }

            if (_tileData.Owner.IsLocal)
            {
                return new Color(0.07f, 0.666f, 0);
            }

            throw new ArgumentException($"Tile ownership not set as expected: {_tileData.Owner}");
        }
    }
}
