using Microsoft.EntityFrameworkCore;
using nabegemu.Database.Interfaces;
using nabegemu.Database.Models;

namespace nabegemu.Database
{
    public class GameRepository : IGameRepository
    {
        public List<Game> GetGames()
        {
            using var context = new GameContext();

            return context.Games.Include(game => game.Players).ToList();
        }

        public Game GetGame(int id)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, id)
                ?? throw new Exception("Game not found");

            context.SaveChanges();

            return game;
        }

        public Game CreateGame(Player player)
        {
            using var context = new GameContext();

            var newGame = new Game();

            newGame.Players = new List<Player>
            {
                CreatePlayer(newGame.GameId, player.Name, true)
            };

            context.Games.Add(newGame);

            context.SaveChanges();

            return newGame;
        }

        public Player AddPlayerToGame(int gameId, string playerName)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            var newPlayer = CreatePlayer(game.GameId, playerName);

            context.Players.Add(newPlayer);

            game.Players.Add(newPlayer);

            context.SaveChanges();

            return newPlayer;
        }

        public List<Player> GetPlayers(int gameId)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            return game.Players;
        }

        public Player GetPlayer(int gameId, Guid playerId)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            var player = game.Players.First(x => x.Id == playerId);

            return player;
        }

        public Player GetActivePlayer(int gameId)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            return game.Players.First(x => x.IsActivePlayer == true);
        }

        public Player SetNextActivePlayer(int gameId)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            var currentActivePlayer = game.Players.First(x => x.IsActivePlayer == true);
            var currentActivePlayerIndex = game.Players.IndexOf(currentActivePlayer);            

            currentActivePlayer.IsActivePlayer = false;

            var nextActivePlayerIndex = (currentActivePlayerIndex + 1) % game.Players.Count;
            game.Players[nextActivePlayerIndex].IsActivePlayer = true;

            context.SaveChanges();

            return game.Players.First(x => x.IsActivePlayer == true);
        }

        public bool SwapWithActiveCard(int gameId, Guid playerId, List<Card> newHand)
        {
            using var context = new GameContext();

            var game = GetAllGameData(context, gameId)
                ?? throw new Exception("Game not found");

            var player = game.Players.First(x => x.Id == playerId);
            var previousHandState = player.KitchenThings.YourHand;
            player.KitchenThings.YourHand = newHand;

            // TODO: Not actually updating db
            context.SaveChanges();

            player = game.Players.First(x => x.Id == playerId);
            if (player.KitchenThings.YourHand != previousHandState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Player CreatePlayer(int gameId, string playerName, bool activePlayer = false)
        {
            var player = new Player
            {
                Name = playerName,
                Code = gameId,
                IsActivePlayer = activePlayer,
            };

            player.KitchenThings = GenerateKitchenThings(player.Id);

            return player;
        }

        private KitchenThings GenerateKitchenThings(Guid playerId)
        {
            KitchenThings kitchenThings = new KitchenThings
            {
                Id = Guid.NewGuid(),
                AssociatedPlayerId = playerId,
            };

            Random random = new Random();
            var selectedCardsIndexes = Enumerable.Range(0, kitchenThings.CompleteDeck.Count)
                .OrderBy(i => random.Next())
                .Take(8)
                .ToList();
            kitchenThings.YourHand = selectedCardsIndexes.Select(i => kitchenThings.CompleteDeck[i]).ToList();

            kitchenThings.DrawDeckCard = kitchenThings.CompleteDeck[random.Next(0, kitchenThings.CompleteDeck.Count)];

            return kitchenThings;
        }

        private Game? GetAllGameData(GameContext context, int gameId)
        {
            var game = context.Games
                .AsNoTracking()
                .First(x => x.GameId == gameId);

            var players = context.Players
                .AsNoTracking()
                .Where(x => x.Code == gameId)
                .ToList();

            foreach (var player in players)
            {
                player.KitchenThings = context.KitchenThings
                    .AsNoTracking()
                    .Include(x => x.DrawDeckCard)
                    .Include(x => x.YourHand)
                    .Include(x => x.YourDiscard)
                    .Include(x => x.PlayerDiscardA)
                    .Include(x => x.PlayerDiscardB)
                    .Include(x => x.PlayerDiscardC)
                    .First(x => x.AssociatedPlayerId == player.Id);
            }

            Game fullGameData = new Game
            {
                GameId = game.GameId,
                Players = players,
            };

            return fullGameData;
        }
    }
}
