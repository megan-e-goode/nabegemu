using nabegemu.Database.Interfaces;
using nabegemu.Database.Models;

namespace nabegemu.Hubs;
public class LobbyHub : Hub
{
    readonly IGameRepository _gameRepository;
    public LobbyHub(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task AddNewPlayer(string playerName, string gameCode)
    {
        var newPlayer = new Player
        {
            Name = playerName,
            Code = int.Parse(gameCode)
        };

        _gameRepository.AddPlayerToGame(int.Parse(gameCode), newPlayer);

        var game = _gameRepository.GetGame(int.Parse(gameCode));

        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{game.GameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{newPlayer.Id}");

        await Clients.Group($"player-{newPlayer.Id}").SendAsync("SetPlayerIdInSession", newPlayer.Id);
        await Clients.Group($"game-{game.GameId}").SendAsync("NewPlayerAdded", game);
    }

    public async Task CreateNewGame(string playerName)
    {
        var player = new Player();
        player.Name = playerName;

        var game = _gameRepository.CreateGame(player);
        var playerId = game.Players.First().Id;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{game.GameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{playerId}");

        await Clients.Group($"player-{playerId}").SendAsync("SetPlayerIdInSession", playerId);
        await Clients.Group($"game-{game.GameId}").SendAsync("NewGameCreated", game);
    }

    public async Task PrepKitchen(string gameCode, Guid playerId)
    {
        var player = _gameRepository.GetPlayer(int.Parse(gameCode), playerId);

        var kitchen = new KitchenThings();

        await Clients.Group($"player-{player.Id}").SendAsync("PrepKitchenComplete", kitchen);
    }
}