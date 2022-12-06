using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Incorporation
{
    public class MainMenu : MonoBehaviour
    {
        private void Start()
        {
            SignalRClient.Connect();
            Debug.Log("Opening Menu");

        }
        public void NewGame()
        {
            SignalRClient.CreateLobby();
            //SceneManager.LoadScene("GameScene");
        }
        public void JoinGame()
        {
            Debug.Log("Joining game...");
        }
        public void Quit()
        {
            Debug.Log("Exiting application...");
            Application.Quit();
        }
    }
}
