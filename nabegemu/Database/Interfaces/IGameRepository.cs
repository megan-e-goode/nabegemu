using nabegemu.Database.Models;

namespace nabegemu.Database.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetGames();

        Game GetGame(int id);

        Game CreateGame(Player player);

        Player AddPlayerToGame(int gameId, string playerName);

        List<Player> GetPlayers(int gameId);

        Player GetPlayer(int gameId, Guid playerId);

        Player GetActivePlayer(int gameId);

        bool SwapWithActiveCard(int gameId, Guid playerId, Card cardToSwap, Card activeCard);
    }
}
