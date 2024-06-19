using nabegemu.Database.Enum;

namespace nabegemu.Database.Models
{
    public class Card
    {
        public Card(CardType type, string name)
        {
            Name = name;
            Type = type;
            Colour = type switch
            {
                CardType.Meat => CardColour.Red,
                CardType.Fish => CardColour.Blue,
                CardType.Veggies => CardColour.Yellow,
                CardType.Noodles => CardColour.Beige,
                CardType.Mushrooms => CardColour.Purple,
                CardType.Carbs => CardColour.Grey,
                CardType.Greens => CardColour.Green,
                CardType.Spices => CardColour.Orange,
                _ => CardColour.Grey
            };
        }

        public CardType Type { get; set; }

        public CardColour Colour { get; set; }

        public string Name { get; set; }
    }
}
