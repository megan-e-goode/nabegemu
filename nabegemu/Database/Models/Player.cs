using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class Player
    {
        public Player()
        {
            Id = Guid.NewGuid();
            Score = 0;
            KitchenThings = new();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [RegularExpression("^\\d{4}$")]
        public int Code { get; set; }

        public int Score { get; set; }

        public KitchenThings KitchenThings { get; set; }
    }
}
