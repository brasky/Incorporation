using UnityEngine.Events;
using UnityEngine;
using Shared;

namespace Assets.GameClient.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "Events/PlayerActionEventChannel")]
    public class PlayerActionEventChannel : SerializableScriptableObject
    {
        [TextArea] public string description;

        public UnityAction<PlayerAction> OnEventRaised;

        public void RaiseEvent(PlayerAction action)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(action);
        }
    }
}
