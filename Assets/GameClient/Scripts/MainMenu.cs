using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Incorporation
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            Application.runInBackground = true;
            SignalRClient.Connect();
        }

        public void NewGame()
        {
            SignalRClient.CreateLobby();
            SceneManager.LoadScene("LobbyScene");

        }

        public void JoinGame()
        {
            var input = FindObjectOfType<TMP_InputField>();
            Debug.Log($"Joining game: {input.text}");
            SignalRClient.JoinGame(input.text);
            SceneManager.LoadScene("LobbyScene");
        }

        public void Quit()
        {
            Debug.Log("Exiting application...");
            Application.Quit();
        }
    }
}
