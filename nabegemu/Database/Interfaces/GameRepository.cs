﻿using Microsoft.EntityFrameworkCore;
using nabegemu.Database.Models;
using System;

namespace nabegemu.Database.Interfaces
{
    public class GameRepository : IGameRepository
    {

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
            using (var context = new GameContext())
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

                context.Games.Add(newGame);

                context.SaveChanges();

                return newGame;
            }
            
        }

        public Player AddPlayerToGame(int gameId, string playerName)
        {
            using (var context = new GameContext())
            {
                var game = context.Games
                    .Include(game => game.Players)
                    .FirstOrDefault(x => x.GameId == gameId) 
                    ?? throw new Exception("Game not found");
                var newPlayer = new Player
                {
                    Name = playerName,
                    Code = game.GameId,
                };

                game.Players.Add(newPlayer);

                context.SaveChanges();

                return newPlayer;
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

        public void AddKitchenThingsToPlayer(int gameId, Guid playerId, KitchenThings kitchenThings)
        {
            throw new NotImplementedException();
        }
    }
}
