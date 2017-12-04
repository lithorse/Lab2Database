namespace Lab2Database.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Lab2Database.GameContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Lab2Database.GameContext context)
        {
            Player p1 = new Player("Otto");
            Player p2 = new Player("Rikard");
            Player p3 = new Player("David");

            context.Players.AddOrUpdate(p1);
            context.Players.AddOrUpdate(p2);
            context.Players.AddOrUpdate(p3);

            Course c1 = new Course("Cool Land", 6);
            Course c2 = new Course("Pretty World", 5);
            Course c3 = new Course("Radical City", 7);

            context.Courses.AddOrUpdate(c1);
            context.Courses.AddOrUpdate(c2);
            context.Courses.AddOrUpdate(c3);

            context.Scores.AddOrUpdate(new Score(4, p1, c2));
            context.Scores.AddOrUpdate(new Score(5, p2, c1));
            context.Scores.AddOrUpdate(new Score(4, p3, c3));
        }
    }
}
