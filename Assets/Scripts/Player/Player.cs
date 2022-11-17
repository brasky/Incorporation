using Incorporation.Assets.ScriptableObjects.Player;
using UnityEngine;

namespace Incorporation.Assets.Scripts.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerEventChannel _playerEventChannel = default;

        private GameManager _gameManager;

        void Start()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        void OnEnable()
        {
            _playerEventChannel.OnEndTurn += EndTurn;
        }

        private void EndTurn()
        {
            _gameManager.EndTurn();
        }
    }
}
