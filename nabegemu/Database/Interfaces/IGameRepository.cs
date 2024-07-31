using nabegemu.Database.Models;

namespace nabegemu.Database.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetGames();

        Game GetGame(int id);

        Game CreateGame(Player player);

        Player AddPlayerToGame(int gameId, string playerName);

        Player GetPlayer(int gameId, Guid playerId);
    }
}
