using Microsoft.EntityFrameworkCore;
using nabegemu.Database.Interfaces;
using nabegemu.Database.Models;
using System;

namespace nabegemu.Database
{
    public class GameRepository : IGameRepository
    {
        private readonly GameContext _context;

        public GameRepository(GameContext context)
        {
            _context = context;
        }

        public List<Game> GetGames()
        {
            return _context.Games.Include(game => game.Players).ToList();
        }

        public Game GetGame(int id)
        {
            var game = _context.Games.Include(game => game.Players).FirstOrDefault(x => x.GameId == id);

            if (game == null)
            {
                // throw error here
                return new Game();
            }

            _context.SaveChanges();

            return game;
        }

        public Game CreateGame(Player player)
        {
            var newGame = new Game();

            newGame.Players = new List<Player>
            {
                new()
                {
                    Name = player.Name,
                    Code = newGame.GameId,
                }
            };

            _context.Games.Add(newGame);

            _context.SaveChanges();

            return newGame;

        }

        public Player AddPlayerToGame(int gameId, string playerName)
        {
            var game = _context.Games
                .Include(game => game.Players)
                .FirstOrDefault(x => x.GameId == gameId)
                ?? throw new Exception("Game not found");

            var newPlayer = new Player
            {
                Name = playerName,
                Code = game.GameId,
            };

            _context.Players.Add(newPlayer);

            game.Players.Add(newPlayer);

            _context.SaveChanges();

            return newPlayer;
        }

        public Player GetPlayer(int gameId, Guid playerId)
        {
            var game = _context.Games.Include(game => game.Players).FirstOrDefault(x => x.GameId == gameId);

            if (game == null)
            {
                throw new Exception("Game not found");
            }

            var player = game.Players.First(x => x.Id == playerId);

            return player;
        }
    }
}
