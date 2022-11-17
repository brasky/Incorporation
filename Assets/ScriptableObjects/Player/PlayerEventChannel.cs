using UnityEngine.Events;
using UnityEngine;

namespace Incorporation.Assets.ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "Player", menuName = "Player/PlayerEventChannel")]
    public class PlayerEventChannel : SerializableScriptableObject
    {
        [TextArea] public string description;

        public UnityAction OnEndTurn;

        public void RaiseEndTurnEvent()
        {
            if (OnEndTurn != null)
                OnEndTurn.Invoke();
        }
    }
}
