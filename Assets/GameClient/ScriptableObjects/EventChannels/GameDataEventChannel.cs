using UnityEngine;
using UnityEngine.Events;

namespace Incorporation.Assets.ScriptableObjects.EventChannels
{
    [CreateAssetMenu(menuName = "Events/GameDataEventChannel")]
    public class GameDataEventChannel : ScriptableObject
    {
        [TextArea] public string description;

        public UnityAction<GameData> OnEventRaised;

        public GameData MostRecentState { get; private set; }

        public void RaiseEvent(GameData state)
        {
            MostRecentState = state;

            if (OnEventRaised != null)
                OnEventRaised.Invoke(state);
        }
    }
}
