using nabegemu.Database.Enum;
using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class Card
    {
        public Card(CardType type, string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            Colour = type switch
            {
                CardType.Meat => CardColour.Red,
                CardType.Fish => CardColour.Blue,
                CardType.Veggies => CardColour.Yellow,
                CardType.Noodles => CardColour.Brown,
                CardType.Mushrooms => CardColour.Purple,
                CardType.Carbs => CardColour.Gray,
                CardType.Greens => CardColour.Green,
                CardType.Spices => CardColour.Orange,
                _ => CardColour.Gray
            };
        }

        [Key]
        public Guid Id { get; init; }

        public CardType Type { get; set; }

        public CardColour Colour { get; set; }

        public string Name { get; set; }
    }
}
