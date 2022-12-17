using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using BestHTTP.SignalRCore;

namespace Incorporation
{
    //TODO: This script is currently in use by MainMenu AND JoinGameMenu. This makes it hard to reason about the script because it is not static,
    //a new object is created when the player switches from the main menu to the join game menu.
    public class MainMenu : MonoBehaviour
    {
        private bool newGameSelected = false;
        private bool joinGameSelected = false;
        private bool joiningGame = false;
        private float _timer = 0.0f;
        private float _timeout = 5.0f;

        private SignalRClient _client => SignalRClient.Instance;

        private void Start()
        {
            Application.runInBackground = true;
        }

        public void NewGame()
        {
            if (_client.Hub.State != ConnectionStates.Connected)
            {
                _client.Connect();
                newGameSelected = true;
            }
        }

        void Update()
        {
            if (newGameSelected && _client.Hub.State == ConnectionStates.Connected)
            {
                newGameSelected = false;
                SceneManager.LoadScene("LobbyScene");
            }

            if (joinGameSelected && !joiningGame && _client.Hub.State == ConnectionStates.Connected)
            {
                joiningGame = true;
                var input = FindObjectOfType<TMP_InputField>();
                Debug.Log($"Joining game: {input.text}");
                _client.JoinGame(input.text);
                SceneManager.LoadScene("LobbyScene");
            }

            if ((newGameSelected || joinGameSelected) && _client.Hub.State != ConnectionStates.Connected)
            {
                _timer += Time.deltaTime;
            }

            if (_timer > _timeout)
            {
                _timer = 0.0f;
                _client.Disconnect();
                newGameSelected = false;
                joinGameSelected = false;
                Debug.Log("Failed to connect to server");
            }
        }

        public void JoinGame()
        {
            if (_client.Hub.State != ConnectionStates.Connected)
            {
                _client.Connect();
                joinGameSelected = true;
            }
        }

        public void Quit()
        {
            Debug.Log("Exiting application...");
            Application.Quit();
        }
    }
}
