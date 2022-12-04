using UnityEngine;

namespace Incorporation
{
    [CreateAssetMenu(fileName = "Lobby", menuName = "ScriptableObjects/Lobby")]
    public class Lobby : ScriptableObject
    {
        private string _lobbyId;
        public string LobbyId { get; set; }
    }
}
