using Microsoft.AspNetCore.SignalR;
using Shared;
using Shared.Players;
using Shared.Tiles;
using System.Collections.Concurrent;

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
            player.IsHost = true;
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
        private async Task SendGameState(string groupId, string channel)
        {
            logger.LogInformation($"Sending game state to: {groupId} - {games[groupId].State}");
            await Clients.Group(groupId).SendAsync(channel, games[groupId]);
        }

        public async Task RequestGameState()
        {
            logger.LogInformation($"Player requested game state: {Context.ConnectionId}");
            var game = games.Values.Where(s => s.Players.Any(p => p.Id == Context.ConnectionId)).First();
            logger.LogInformation($"Returning state for game {game.Id} - {game.State}");
            await Clients.Caller.SendAsync("RefreshGameState", game);
        }

        public async Task JoinGame(string groupId) 
        {
            logger.LogInformation($"player {Context.ConnectionId} is joining game {groupId}");
            if (!games.TryGetValue(groupId, out var game)) return;
            game.Players.Add(new PlayerData(Context.ConnectionId));
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            //Need to tell caller their connection id.
            await Clients.Caller.SendAsync("JoinedGame", Context.ConnectionId);
            await SendGameState(groupId, "RefreshGameState");
        }

        public async Task StartGame(string groupId)
        {
            logger.LogInformation($"player {Context.ConnectionId} is starting game {groupId}");
            var game = games[groupId];
            if (game.State == GameState.LOBBY && game.Players.Where(p => p.Id == Context.ConnectionId).First().IsHost)
            {
                game.State = GameState.SETUP;
                await SendGameState(groupId, "RefreshGameState");

                Server.SetupGame(game);

                game.State = GameState.READYCHECK;
                await SendGameState(groupId, "ReadyCheck");
            }
        }

        public async Task Ready(string groupId)
        {
            logger.LogInformation($"player {Context.ConnectionId} is ready");
            var game = games[groupId];
            game.Players.Where(p => p.Id == Context.ConnectionId).First().IsReady = true;

            if (game.Players.All(p => p.IsReady))
            {
                game.State = GameState.SETUPCOMPLETE;
            }

            logger.LogInformation($"{game.Players.Count(p => p.IsReady)} players ready");

            await SendGameState(groupId, "RefreshGameState");
        }
    }

    public static class Server
    {
        public static void SetupGame(ServerState state)
        {
            GenerateTiles(state);
            AssignStartingTiles(state);
        }

        private static void AssignStartingTiles(ServerState state)
        {
            var rand = new Random();
            foreach (var player in state.Players)
            {
                var unownedTiles = state.Tiles.Where(t => t.Owner is null).ToList();
                unownedTiles[rand.Next(0, unownedTiles.Count-1)].Owner = player;
            }
        }

        private static void GenerateTiles(ServerState state)
        {
            for (var x = 0; x < state.MapWidth; x++)
            {
                for (var y = 0; y < state.MapWidth; y++)
                {
                    var data = new TileData(x, y);
                    state.Tiles.Add(data);
                    state.TileMap[x, y] = data;
                }
            }
        }
    }
}
