using UnityEngine.Events;
using UnityEngine;
using Incorporation.Assets.Scripts.TileGrid;

namespace Incorporation.Assets.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "Events/TileEventChannel")]
    public class TileEventChannel : SerializableScriptableObject
    {
        [TextArea] public string description;

        public UnityAction<Tile> OnEventRaised;

        public void RaiseEvent(Tile tile)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(tile);
        }
    }
}
