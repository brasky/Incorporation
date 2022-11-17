using UnityEngine;
using UnityEngine.Events;

namespace Incorporation
{
    public class TileEventChannel : SerializableScriptableObject
    {
        [TextArea] public string description;

        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke();
        }
    }
}
