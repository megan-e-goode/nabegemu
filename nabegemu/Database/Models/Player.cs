using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class Player
    {
        public Player()
        {
            Id = new Guid();
            Score = 0;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        [RegularExpression("^\\d{4}$")]
        public int Code { get; set; }

        public int Score { get; set; }
    }
}
