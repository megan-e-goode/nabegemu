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

        await Clients.All.SendAsync("NewPlayerAdded", game);
    }

    public async Task CreateNewGame(string playerName)
    {
        var player = new Player();
        player.Name = playerName;

        var game = _gameRepository.CreateGame(player);

        await Clients.All.SendAsync("NewGameCreated", game);
    }
}