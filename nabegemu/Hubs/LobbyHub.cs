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
        var newPlayer = _gameRepository.AddPlayerToGame(int.Parse(gameCode), playerName);

        var game = _gameRepository.GetGame(int.Parse(gameCode));

        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{game.GameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{newPlayer.Id}");

        await Clients.Group($"player-{newPlayer.Id}").SendAsync("SetPlayerDataInSession", newPlayer);
        await Clients.Group($"game-{game.GameId}").SendAsync("NewPlayerAdded", game);
    }

    public async Task CreateNewGame(string playerName)
    {
        var player = new Player();
        player.Name = playerName;

        var game = _gameRepository.CreateGame(player);
        player = game.Players.First();

        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{game.GameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{player.Id}");

        await Clients.Group($"player-{player.Id}").SendAsync("SetPlayerDataInSession", player);
        await Clients.Group($"game-{game.GameId}").SendAsync("NewGameCreated", game);
    }

    public async Task PrepKitchen(int gameCode, Guid playerId)
    {
        var player = _gameRepository.GetPlayer(gameCode, playerId);

        var kitchen = new KitchenThings();

        await Clients.Group($"player-{player.Id}").SendAsync("PrepKitchenComplete", kitchen);
    }
}