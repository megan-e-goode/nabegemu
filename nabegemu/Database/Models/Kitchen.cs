﻿using nabegemu.Database.Enum;
using System.ComponentModel.DataAnnotations;

namespace nabegemu.Database.Models
{
    public class KitchenThings
    {
        [Key]
        public Guid Id { get; init; }

        public Card ActiveCard { get; set; }

        public Card DrawDeckCard { get; set; }

        public List<Card> YourHand { get; set; } = [];

        public List<Card> YourDiscard { get; set; } = [];

        public List<Card> PlayerDiscardA { get; set; } = [];

        public List<Card> PlayerDiscardB { get; set; } = [];

        public List<Card> PlayerDiscardC { get; set; } = [];

        public List<Card> CompleteDeck { get; private set; } =
            [
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
            ];
    }
}
