using Microsoft.EntityFrameworkCore;
using nabegemu.Database.Models;

namespace nabegemu.Database
{
    public class GameContext : DbContext
    {
        protected override void OnConfiguring
            (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "NabeGemuDb");
        }
        
        public DbSet<Game> Games { get; set; }
    }
}
