using nabegemu.Database.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class KitchenThings
    {
        public KitchenThings()
        {
            Random random = new Random();
            var selectedCards = Enumerable.Range(0, CompleteDeck.Count)
                                  .OrderBy(i => random.Next())
                                  .Take(8)
                                  .ToList();
            YourHand = selectedCards.Select(i => CompleteDeck[i]).ToList();

            var alteredDeck = CompleteDeck.Except(YourHand).ToList();

            YourDiscard = new List<Card>
            {
                alteredDeck[random.Next(0, alteredDeck.Count)]
            };

            PlayerDiscardA = new List<Card>
            {
                alteredDeck[random.Next(0, alteredDeck.Count -1)]
            };

            PlayerDiscardB = new List<Card>
            {
                alteredDeck[random.Next(0, alteredDeck.Count -2)]
            };

            PlayerDiscardC = new List<Card>
            {
                alteredDeck[random.Next(0, alteredDeck.Count -3)]
            };

            DrawDeck = alteredDeck;

            ActiveCard = DrawDeck[random.Next(0, DrawDeck.Count)];

            Id = Guid.NewGuid();
        }

        private List<Card> CompleteDeck = new List<Card>
        {
            new Card(CardType.Meat, "Rolled Beef"),
            new Card(CardType.Meat, "Beef Rib"),
            new Card(CardType.Meat, "Steak"),
            new Card(CardType.Fish, "Shrimp"),
            new Card(CardType.Fish, "Lobster"),
            new Card(CardType.Fish, "Tuna"),
            new Card(CardType.Veggies, "Corn"),
            new Card(CardType.Veggies, "Potato"),
            new Card(CardType.Veggies, "Carrot"),
            new Card(CardType.Noodles, "Rice Noodle"),
            new Card(CardType.Noodles, "Soba Noodle"),
            new Card(CardType.Noodles, "Udon Noodle"),
            new Card(CardType.Mushrooms, "Shiitake"),
            new Card(CardType.Mushrooms, "Enoki"),
            new Card(CardType.Mushrooms, "Matsutake"),
            new Card(CardType.Carbs, "Tofu"),
            new Card(CardType.Carbs, "Dumplings"),
            new Card(CardType.Carbs, "Rice Cake"),
            new Card(CardType.Greens, "Bok Choy"),
            new Card(CardType.Greens, "Cabbage"),
            new Card(CardType.Greens, "Spring Onion"),
            new Card(CardType.Spices, "Garlic"),
            new Card(CardType.Spices, "Ginger"),
            new Card(CardType.Spices, "Dari Cloves")
        };

        [Key]
        public Guid Id { get; init; }

        public Card ActiveCard { get; set; }

        public List<Card> DrawDeck { get; set; } = new List<Card>();

        public List<Card> YourHand { get; set; } = new List<Card>();

        public List<Card> YourDiscard { get; set; } = new List<Card>();

        public List<Card> PlayerDiscardA { get; set; } = new List<Card>();

        public List<Card> PlayerDiscardB { get; set; } = new List<Card>();

        public List<Card> PlayerDiscardC { get; set; } = new List<Card>();
    }
}
