using Incorporation.Assets.ScriptableObjects.EventChannels;
using Shared.Players;
using Shared.Resources;
using Shared.Tiles;
using System.Text;
using TMPro;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        public TileData _tileData;

        private bool _detailPaneSelected = false;
        private Color _unownedTileColor = new Color(0.86f, 0.86f, 0.86f);
        public Color OwnerColor { get; set; } = new Color(0.0f, 0.0f, 0.0f);

        public PlayerData Owner => _tileData.Owner;
        public bool IsImproved => _tileData.IsImproved;
        public int Yield => _tileData.Yield;
        public ResourceCost[] ResourceCosts => _tileData.ResourceCosts;

        [SerializeField]
        private TileEventChannel _tileClickEventChannel;


        public Resource[] Resources => _tileData.Resources;
        public int Price => _tileData.Price;

        public string GetResourceCostsAsString()
        {
            var sb = new StringBuilder();

            foreach(var resource in ResourceCosts)
            {
                sb.Append(resource.Resource);
                sb.Append(": ");
                sb.Append(resource.Quantity);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        // Start is called before the first frame update
        void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            if (_tileData.Owner is null)
                _renderer.color = _unownedTileColor;
            else
                _renderer.color = OwnerColor;

            var tileText = GetComponentInChildren<TextMeshProUGUI>();
            tileText.text = Resources[0].ToString();
        }

        public void SetDetailsPaneSelected()
        {
            var prev = _renderer.color;
            _renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
            _detailPaneSelected = true;
        }

        public void SetDetailsPaneDeselected()
        {
            if (_tileData.Owner is null)
                _renderer.color = _unownedTileColor;
            else
                _renderer.color = OwnerColor;
            _detailPaneSelected = false;
        }

        private void OnMouseOver()
        {
            var prev = _renderer.color;
            _renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
        }

        private void OnMouseExit()
        {
            if (_detailPaneSelected)
                return;

            if (_tileData?.Owner is null)
            {
                _renderer.color = _unownedTileColor;
            }
            else
            {
                _renderer.color = OwnerColor;
            }
        }

        private void OnMouseDown()
        {
            _tileClickEventChannel.RaiseEvent(this);
        }
    }
}
