using nabegemu.Database.Models;

namespace nabegemu.Database.Interfaces;

public interface ILobbyHub
{
    Task AddNewPlayer(string playerName, string gameCode);

    Task CreateNewGame(string playerName);

    Task PrepKitchen(int gameCode, Guid playerId);

    Task SetActivePlayerInSession(int gameCode);

    Task SwapWithActiveCard(int gameCode, Guid playerId, Card cardToSwap, Card activeCard);
}
