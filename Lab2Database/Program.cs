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
                string input = "";
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        EnterName(context);
                        break;
                    case "2":
                        PrintPlayers(context);
                        break;
                    case "3":
                        PrintCourses(context);
                        break;
                    case "4":
                        PrintScores(context);
                        break;
                    case "5":
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine(input + " is not a recognized input");
                        break;
                }
            }
        }

        public static void EnterName(GameContext context)
        {
            Console.Clear();
            Console.WriteLine("Please enter name:");
            string name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine($"Checking database for {name}...");
            var query = from player in context.Players
                        select player;
            bool nameFound = false;
            int playerId = -1;
            foreach (var player in query)
            {
                if (player.Name == name)
                {
                    nameFound = true;
                    playerId = player.PlayerId;
                }
            }
            if (nameFound)
                PlayerMenu(context, name, playerId);
            else
                NewPlayer(context, name);
        }

        public static void PlayerMenu(GameContext context, string name, int playerId)
        {
            Console.Clear();
            Console.WriteLine("Collecting your stats...");
            bool hasScores = (from score in context.Scores where score.Player.PlayerId == playerId select score).Count() > 0;
            Console.Clear();
            Console.WriteLine($"Welcome {name}!");
            Console.WriteLine();
            if (hasScores)
            {
                var query = from score in context.Scores
                            join course in context.Courses on
                            score.Course equals course
                            where score.Player.PlayerId == playerId
                            select score.ScoreId + "  " + course.Name + "\t" + (course.Birds - score.MovesLeft) + "\t" + score.MovesLeft;
                Console.WriteLine("Here are your scores on courses you have beaten:");
                Console.WriteLine("Id  Course\tMoves\t\tMoves Left");
                foreach (var scores in query)
                {
                    Console.WriteLine(scores);
                }
                Console.WriteLine();
                Console.WriteLine($"Total score: {(from score in context.Scores where score.Player.PlayerId == playerId select score.MovesLeft).Sum()}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("You have not beaten any courses!");
                Console.WriteLine();
            }
            Console.WriteLine("What do you want to do?");
            Console.WriteLine("1: Update score\n2: Add new course\n3: Return to previous menu");
            string input = "";
            input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    UpdateScore(context, playerId);
                    break;
                case "2":
                    PrintPlayers(context);
                    break;
                case "3":
                    PrintCourses(context);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine(input + " is not a recognized input");
                    break;
            }
        }
        public static void NewPlayer(GameContext context, string name)
        {
            Console.Clear();
            Console.WriteLine("New player detected, adding to database...");
            context.Players.Add(new Player(name));
            context.SaveChanges();
            int playerId = (from player in context.Players where player.Name == name select player.PlayerId).Single();
            Console.WriteLine($"{name} added to database");
            Console.WriteLine("Press any key to continue to Player Menu...");
            Console.ReadKey();
            PlayerMenu(context, name, playerId);
        }

        public static void UpdateScore(GameContext context, int playerId)
        {
            Console.Clear();
            Console.WriteLine("Loading Courses in Database...");
            var query = from course in context.Courses
                        select course;
            Console.Clear();
            Console.WriteLine("Enter Id for course you wish to update your score for:");
            foreach (var course in query)
            {
                Console.WriteLine(course.CourseId + " " + course.Name);
            }
            Console.WriteLine();
            int input = -1;
            try
            {
                input = Int32.Parse(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            bool validCourseId = (from course in context.Courses where course.CourseId == input select course).Count() == 1;
            if (validCourseId)
            {
                Console.WriteLine($"What will your new score be? Maximum is {(from course in context.Courses where course.CourseId == input select course.Birds).Single()}");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Course with Id {input} does not exist");
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
            Console.WriteLine();
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
            Console.WriteLine();
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
                        select score.ScoreId + "  " + course.Name + "\t" + player.Name + "\t\t" + score.MovesLeft;
            Console.Clear();
            Console.WriteLine("Current Scores in Database:");
            Console.WriteLine("Id  Course\t\tPlayer\t\tScore");
            foreach (var scores in query)
            {
                Console.WriteLine(scores);
            }
            Console.WriteLine();
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
        [Required]
        [MaxLength(16)]
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
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [Required]
        public int Birds { get; set; }

        public virtual ICollection<Score> Scores { get; set; }
    }

    public class Score
    {
        public Score()
        {

        }
        public Score(int movesLeft, Player player, Course course)
        {
            MovesLeft = movesLeft;
            Player = player;
            Course = course;
        }
        [Key]
        public int ScoreId { get; set; }
        [Required]
        public int MovesLeft { get; set; }
        public virtual Player Player { get; set; }
        public virtual Course Course { get; set; }
    }
}
