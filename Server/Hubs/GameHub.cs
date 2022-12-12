using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.Players;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Server.Hubs
{
    public class GameHub : Hub
    {
        private static readonly ConcurrentDictionary<string, ServerState> games = new();
        private readonly ILogger<GameHub> logger;

        public GameHub(ILogger<GameHub> logger)
        {
            this.logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            logger.LogInformation($"New client connection: {Context.ConnectionId}");
            return base.OnConnectedAsync();
        }

        private Task NewLobbyCreated(string groupId)
        {
            logger.LogInformation($"New lobby created {groupId}");
            return SendGameState(groupId, nameof(NewLobbyCreated));
        }

        public async Task CreateLobby()
        {
            string uuid = Guid.NewGuid().ToString();
            PlayerData player = new PlayerData(Context.ConnectionId);
            games[uuid] = new ServerState(uuid, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, uuid);
            await NewLobbyCreated(uuid);
        }

        public async Task SendMessage(string msg)
            => await Clients.All.SendAsync("ChatMessage", msg);

        private Task SendGameState(string groupId, string channel)
        {
            logger.LogInformation($"Sending game state to: {groupId}");
            return Clients.Group(groupId).SendAsync(channel, games[groupId]);
        }

        public async Task RequestGameState()
        {
            logger.LogInformation($"Player requested game state: {Context.ConnectionId}");
            var game = games.Values.Where(s => s.Players.Any(p => p.Id == Context.ConnectionId)).First();
            logger.LogInformation($"Returning state for game {game.Id}");
            await Clients.Caller.SendAsync("RefreshGameState", game);
        }

        public async Task JoinGame(string Id) 
        {
            logger.LogInformation($"player {Context.ConnectionId} is joining game {Id}");
            if (!games.TryGetValue(Id, out var game)) return;
            game.Players.Add(new PlayerData(Context.ConnectionId));

            //Need to tell caller their connection id.
            await Clients.Caller.SendAsync("JoinedGame", Context.ConnectionId);
        }
    }

}
