using UnityEngine;
using UnityEngine.Events;

namespace Incorporation.Assets.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "Events/GameDataEventChannel")]
    public class GameDataEventChannel : ScriptableObject
    {
        [TextArea] public string description;

        public UnityAction<GameData> OnEventRaised;

        public void RaiseEvent(GameData state)
        {
            if (OnEventRaised != null)
                OnEventRaised.Invoke(state);
        }
    }
}
