using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Incorporation
{
    public enum GameState
    {
        START,
        PLAYERTURN,
        ENEMYTURN,
        WON,
        LOST
    }

    public class GameManager : MonoBehaviour
    {

        public GameState state;
        // Start is called before the first frame update
        void Start()
        {
            state = GameState.START;

            state = GameState.PLAYERTURN;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool IsPlayerTurn() 
        {
            return state == GameState.PLAYERTURN;
        }

        public void EndTurn()
        {
            state = GameState.ENEMYTURN;

        }
    }
}
