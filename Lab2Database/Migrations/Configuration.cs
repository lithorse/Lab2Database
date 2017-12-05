namespace Lab2Database.Migrations
{
    using System;
    using System.Collections.Generic;
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
            Player p4 = new Player("Joe");
            Player p5 = new Player("Jane");
            Player p6 = new Player("Jesus");
            Player p7 = new Player("Trump");
            Player p8 = new Player("Adams");
            Player p9 = new Player("Obama");
            Player p10 = new Player("Madison");
            List<Player> playerList = new List<Player>() { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 };

            foreach (var player in playerList)
            {
                context.Players.AddOrUpdate(player);
            }

            //context.Players.AddOrUpdate(p1);
            //context.Players.AddOrUpdate(p2);
            //context.Players.AddOrUpdate(p3);

            Course c1 = new Course("Cool Land   ", 6);
            Course c2 = new Course("Pretty World", 5);
            Course c3 = new Course("Radical City", 7);

            context.Courses.AddOrUpdate(c1);
            context.Courses.AddOrUpdate(c2);
            context.Courses.AddOrUpdate(c3);

            context.Scores.AddOrUpdate(new Score(4, p1, c2));
            context.Scores.AddOrUpdate(new Score(5, p2, c1));
            context.Scores.AddOrUpdate(new Score(4, p3, c3));
            context.Scores.AddOrUpdate(new Score(3, p4, c1));
            context.Scores.AddOrUpdate(new Score(1, p5, c2));
            context.Scores.AddOrUpdate(new Score(6, p6, c3));
            context.Scores.AddOrUpdate(new Score(5, p7, c1));
            context.Scores.AddOrUpdate(new Score(4, p8, c2));
            context.Scores.AddOrUpdate(new Score(3, p9, c3));
            context.Scores.AddOrUpdate(new Score(2, p10, c1));
        }
    }
}
