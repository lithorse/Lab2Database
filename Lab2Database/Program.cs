using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2Database
{
    class Program
    {
        static void Main(string[] args)
        {
            GameContext context = new GameContext();
            context.Players.Count();
        }
    }

    public class GameContext : DbContext
    {
        private const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AngryBirds;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public GameContext() : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Score>()
                .HasRequired(s => s.Player)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.PlayerId);
            modelBuilder.Entity<Score>()
                .HasRequired(s => s.Map)
                .WithMany(p => p.Scores)
                .HasForeignKey(s => s.MapId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Map> Maps { get; set; }
    }

    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }

    public class Map
    {
        [Key]
        public int MapId { get; set; }
        [Required]
        public string Name  { get; set; }
        [Required]
        public int Birds { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }

    public class Score
    {
        public int ScoreId { get; set; }
        public int PlayerId { get; set; }
        public int MapId { get; set; }
        public int Moves { get; set; }
        public virtual Player Player { get; set; }
        public virtual Map Map { get; set; }
    }
}
