namespace Server.Game
{
    [Serializable]
    public class GameState
    {
        public String name { get; set; }
        public int turnNumber { get; set; }
        public Boolean isPlayerTurn { get; set; }

        public GameState() 
        {
            name = "Test Game State";
            turnNumber = 0;
            isPlayerTurn = false;
        }
    }
}
