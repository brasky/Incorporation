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

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            logger.LogInformation($"Client disconnected: {Context.ConnectionId}");

            return base.OnDisconnectedAsync(exception);
        }

        private Task NewLobbyCreated(string groupId)
        {
            logger.LogInformation($"New lobby created {groupId}");
            return SendGameState(groupId, nameof(NewLobbyCreated));
        }

        public async Task CreateLobby()
        {
            string groupId = Guid.NewGuid().ToString();
            PlayerData player = new PlayerData(Context.ConnectionId);
            games[groupId] = new ServerState(groupId, player);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            await NewLobbyCreated(groupId);
        }

        public async Task SendMessage(string msg)
            => await Clients.All.SendAsync("ChatMessage", msg);

        /// <summary>
        /// Sends the game state for a group (lobby) on a channel
        /// </summary>
        /// <param name="groupId">the group/lobby guid</param>
        /// <param name="channel">Where the client is listening, RefreshGameState or JoinedGame</param>
        /// <returns></returns>
        private Task SendGameState(string groupId, string channel)
        {
            logger.LogInformation($"Sending game state to: {groupId} - {games[groupId].State}");
            return Clients.Group(groupId).SendAsync(channel, games[groupId]);
        }

        public async Task RequestGameState()
        {
            logger.LogInformation($"Player requested game state: {Context.ConnectionId}");
            var game = games.Values.Where(s => s.Players.Any(p => p.Id == Context.ConnectionId)).First();
            logger.LogInformation($"Returning state for game {game.Id}");
            await Clients.Caller.SendAsync("RefreshGameState", game);
        }

        public async Task JoinGame(string groupId) 
        {
            logger.LogInformation($"player {Context.ConnectionId} is joining game {groupId}");
            if (!games.TryGetValue(groupId, out var game)) return;
            game.Players.Add(new PlayerData(Context.ConnectionId));

            //Need to tell caller their connection id.
            await Clients.Caller.SendAsync("JoinedGame", Context.ConnectionId);
            await SendGameState(groupId, "RefreshGameState");
        }

        public async Task StartGame(string groupId)
        {
            logger.LogInformation($"player {Context.ConnectionId} is starting game {groupId}");
            var game = games[groupId];
            game.State = GameState.SETUP;
            await SendGameState(groupId, "RefreshGameState");
        }
    }

}
