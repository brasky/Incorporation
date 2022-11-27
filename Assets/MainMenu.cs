using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Incorporation
{
    public class MainMenu : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene("GameScene");
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
