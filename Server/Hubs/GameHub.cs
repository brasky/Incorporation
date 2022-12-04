using Microsoft.AspNetCore.SignalR;
using Server.Game;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly Dictionary<string, ServerState> games = new();
        private Task RefreshGameState(string groupId)
        {
            ServerState gameState = games[groupId];
            return Clients.Group(groupId).SendAsync("RefreshGameState", gameState);
        }
        public async Task CreateLobby()
        {
            string uuid = Guid.NewGuid().ToString();
            ServerState.Player player = new(Context.ConnectionId);
            games[uuid] = new ServerState(uuid, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, uuid);
            await RefreshGameState(uuid);
        }
        public async Task SendMessage(string msg)
            => await Clients.All.SendAsync("ChatMessage", msg);
    }

}
