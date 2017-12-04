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
            MainSelection(context);
        }

        public static void MainSelection(GameContext context)
        {
            while (true)
            {
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1: Enter Name\n2: Print Players\n3: Print Courses\n4: Print Scores\n5: Quit");
                int input = -1;
                try
                {
                    input = Int32.Parse(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                switch (input)
                {
                    case 1:

                        break;
                    case 2:
                        PrintPlayers(context);
                        break;
                    case 3:
                        PrintCourses(context);
                        break;
                    case 4:
                        PrintScores(context);
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine(input + " is not a valid input");
                        break;
                }
            }
        }

        public static void PrintPlayers(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("Loading Players in Database...");
            var query = from player in context.Players
                        select player;
            Console.Clear();
            Console.WriteLine("Current Players in Database:");
            foreach (var player in query)
            {
                Console.WriteLine(player.PlayerId + " " + player.Name);
            }
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
        }

        public static void PrintCourses(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("Loading Courses in Database...");
            var query = from course in context.Courses
                        select course;
            Console.Clear();
            Console.WriteLine("Current Courses in Database:");
            foreach (var course in query)
            {
                Console.WriteLine(course.CourseId + " " + course.Name);
            }
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
        }

        public static void PrintScores(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("Loading Scores in Database...");
            var query = from score in context.Scores
                        join course in context.Courses on
                        score.Course.CourseId equals course.CourseId
                        join player in context.Players on
                        score.Player.PlayerId equals player.PlayerId
                        select score.ScoreId + " " + course.Name + " " + player.Name + " " + score.Moves;
            Console.Clear();
            Console.WriteLine("Current Scores in Database:");
            foreach (var scores in query)
            {
                Console.WriteLine(scores);
            }
            Console.WriteLine("Press any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
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
                .WithMany(p => p.Scores);
            modelBuilder.Entity<Score>()
                .HasRequired(s => s.Course)
                .WithMany(p => p.Scores);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Score> Scores { get; set; }
    }

    public class Player
    {
        public Player()
        {

        }
        public Player(string name)
        {
            Name = name;
        }
        [Key]
        public int PlayerId { get; set; }
        [Required][MaxLength(16)]
        public string Name { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }

    public class Course
    {
        public Course()
        {

        }
        public Course(string name, int birds)
        {
            Name = name;
            Birds = birds;
        }
        [Key]
        public int CourseId { get; set; }
        [Required][MaxLength(32)]
        public string Name  { get; set; }
        [Required]
        public int Birds { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }

    public class Score
    {
        public Score()
        {

        }
        public Score(int moves, Player player, Course course)
        {
            Moves = moves;
            Player = player;
            Course = course;
        }
        [Key]
        public int ScoreId { get; set; }
        [Required]
        public int Moves { get; set; }
        public virtual Player Player { get; set; }
        public virtual Course Course { get; set; }
    }
}
