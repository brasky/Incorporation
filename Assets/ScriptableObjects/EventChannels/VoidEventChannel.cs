using UnityEngine.Events;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "Events/VoidEventChannel")]
    public class VoidEventChannel : SerializableScriptableObject
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
