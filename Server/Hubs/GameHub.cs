using Microsoft.AspNetCore.SignalR;
using Server.Game;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        public GameState GetState()
        {
            return new GameState();
        }
        public async Task SendMessage(string msg)
            => await Clients.All.SendAsync("ChatMessage", msg);
    }

}
