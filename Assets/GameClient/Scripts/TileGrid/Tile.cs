using Incorporation.Assets.ScriptableObjects.EventChannels;
using Incorporation.Assets.Scripts.Players;
using Incorporation.Assets.Scripts.Resources;
using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Incorporation.Assets.Scripts.TileGrid
{
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private TileData _tileData;

        private bool _detailPaneSelected = false;

        [SerializeField]
        private TileEventChannel _tileClickEventChannel;

        [SerializeField]
        private int maxPossibleYield = 10;

        public ResourceCost[] ResourceCosts { get; private set; }

        public bool IsImproved { get; private set; } = false;
        public Player Owner => _tileData.Owner;
        public Resource[] Resources => _tileData.Resources;
        public int Yield { get; private set; }
        public int Price => _tileData.Price;

        public void Improve() => IsImproved = true;

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
            _tileData = GetComponent<TileData>();

            var randomResource = (Resource)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Resource)).Length);
            _tileData.Resources = new Resource[1] { randomResource };

            Yield = UnityEngine.Random.Range(1, maxPossibleYield);

            //For now just setting the resource cost equal to the random resource and the yield.
            ResourceCosts = new ResourceCost[1];
            ResourceCosts[0] = new ResourceCost(randomResource, Yield);

            var tileText = GetComponentInChildren<TextMeshProUGUI>();
            tileText.text = Resources[0].ToString();
        }

        void Start()
        {
            _renderer.color = Owner.Color;
        }

        public void SetOwner(Player owner)
        {
            _tileData.Owner = owner;
            _renderer.color = Owner.Color;
        }

        public void SetDetailsPaneSelected()
        {
            var prev = _renderer.color;
            _renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
            _detailPaneSelected = true;
        }

        public void SetDetailsPaneDeselected()
        {
            _renderer.color = Owner.Color;
            _detailPaneSelected = false;
        }

        private void OnMouseOver()
        {
            var prev = _renderer.color;
            _renderer.color = new Color(prev.r, prev.g, prev.b, .85f);
        }

        private void OnMouseExit()
        {
            if (!_detailPaneSelected)
                _renderer.color = Owner.Color;
        }

        private void OnMouseDown()
        {
            _tileClickEventChannel.RaiseEvent(this);
        }
    }
}
