using nabegemu.Database.Models;

namespace nabegemu.Database.Interfaces
{
    public interface IGameRepository
    {
        List<Game> GetGames();

        Game GetGame(int id);

        Game CreateGame(Player player);

        void AddPlayerToGame(int gameId, Player player);
    }
}
