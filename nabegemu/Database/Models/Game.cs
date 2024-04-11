namespace nabegemu.Database.Models
{
    public class Game
    {
        public int GameId { get; set; }

        public List<Player> Players { get; set; }
    }
}
