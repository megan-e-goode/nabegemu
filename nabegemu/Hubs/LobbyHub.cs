using nabegemu.Database.Interfaces;
using nabegemu.Database.Models;

namespace nabegemu.Hubs;
public class LobbyHub : Hub, ILobbyHub
{
    readonly IGameRepository _gameRepository;
    readonly ILogger<LobbyHub> _logger;

    public LobbyHub(IGameRepository gameRepository, ILogger<LobbyHub> logger)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task AddNewPlayer(string playerName, string gameCode)
    {
        var newPlayer = _gameRepository.AddPlayerToGame(int.Parse(gameCode), playerName);

        var game = _gameRepository.GetGame(int.Parse(gameCode));

        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{game.GameId}");
        await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{newPlayer.Id}");

        await Clients.Caller.SendAsync("SetPlayerDataInSession", newPlayer);
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

        await Clients.Caller.SendAsync("SetPlayerDataInSession", player);
        await Clients.Group($"game-{game.GameId}").SendAsync("NewGameCreated", game);
    }

    public async Task PrepKitchen(int gameCode, Guid playerId)
    {
        var player = _gameRepository.GetPlayer(gameCode, playerId);

        await Clients.Caller.SendAsync("PrepKitchenComplete", player);
    }

    // TODO: Remove if not needed?
    public async Task SetActivePlayerInSession(int gameCode)
    {
        var activePlayer = _gameRepository.GetActivePlayer(gameCode);
        var inactivePlayers = _gameRepository.GetPlayers(gameCode).Where(player => player.IsActivePlayer == false);

        await Clients.Group($"player-{activePlayer.Id}").SendAsync("SetActivePlayerInSession", true);

        foreach (var player in inactivePlayers)
        {
            await Clients.Group($"player-{player.Id}").SendAsync("SetInactivePlayerInSession", false);
        }
    }

    public async Task SwapWithActiveCard(int gameCode, Guid playerId, Card cardToSwap, Card activeCard)
    {
        var game = _gameRepository.GetGame(gameCode);
        var player = game.Players.First(x => x.Id == playerId);
        var result = _gameRepository.SwapWithActiveCard(gameCode, playerId, cardToSwap, activeCard);

        await Clients.Caller.SendAsync("SwapWithActiveCardComplete", result);
        await Clients.All.SendAsync("ResetKitchen");
    }
}