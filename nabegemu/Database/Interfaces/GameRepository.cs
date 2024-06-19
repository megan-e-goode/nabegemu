using Microsoft.EntityFrameworkCore;
using nabegemu.Database.Models;
using System;

namespace nabegemu.Database.Interfaces
{
    public class GameRepository : IGameRepository
    {
        /*public GameRepository()
        {
            using (var context = new GameContext())
            {
                var game = new Game
                {
                    GameId = 1235,
                    Players = new List<Player>
                    {
                        new Player("Meg", 1235)
                    }
                };

                context.Games.Add(game);
                context.SaveChanges();
            }
        }*/

        public List<Game> GetGames()
        {
            using (var context = new GameContext())
            {
                return context.Games.Include(game => game.Players).ToList();
            }
        }

        public Game GetGame(int id)
        {
            using (var context = new GameContext())
            {
                var game = context.Games.Include(game => game.Players).FirstOrDefault(x => x.GameId == id);

                if (game == null)
                {
                    // throw error here
                    return new Game();
                }

                context.SaveChanges();

                return game;
            }
        }

        public Game CreateGame(Player player)
        {
            int newGameId = new Random().Next(1000, 10000);

            using (var context = new GameContext())
            {
                var newGame = new Game
                {
                    GameId = newGameId,
                    Players = new List<Player>
                    {
                        new Player
                        {
                            Name = player.Name,
                            Code = newGameId,
                        }
                    }
                };

                context.Games.Add(newGame);

                context.SaveChanges();

                return newGame;
            }
            
        }

        public void AddPlayerToGame(int gameId, Player player)
        {
            using (var context = new GameContext())
            {
                var game = context.Games
                    .Include(game => game.Players)
                    .FirstOrDefault(x => x.GameId == gameId);

                if (game == null)
                {
                    throw new Exception("Game not found");
                } else
                {
                    game.Players.Add(player);
                    context.SaveChanges();
                }
            }
        }

        public Player GetPlayer(int gameId, Guid playerId)
        {
            using (var context = new GameContext())
            {
                var game = context.Games.Include(game => game.Players).FirstOrDefault(x => x.GameId == gameId);

                if (game == null)
                {
                    throw new Exception("Game not found");
                }

                var player = game.Players.First(x => x.Id == playerId);

                return player;
            }
        }
    }
}
