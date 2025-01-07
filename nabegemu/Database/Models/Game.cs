using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class Game
    {
        public Game()
        {
            GameId = new Random().Next(1000, 10000);
        }

        [Key]
        public int GameId { get; init; }

        public List<Player> Players { get; set; }
    }
}
