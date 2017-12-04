namespace Lab2Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        CourseId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 32),
                        Birds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CourseId);
            
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        ScoreId = c.Int(nullable: false, identity: true),
                        Moves = c.Int(nullable: false),
                        Course_CourseId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScoreId)
                .ForeignKey("dbo.Courses", t => t.Course_CourseId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_PlayerId, cascadeDelete: true)
                .Index(t => t.Course_CourseId)
                .Index(t => t.Player_PlayerId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 16),
                    })
                .PrimaryKey(t => t.PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "Player_PlayerId", "dbo.Players");
            DropForeignKey("dbo.Scores", "Course_CourseId", "dbo.Courses");
            DropIndex("dbo.Scores", new[] { "Player_PlayerId" });
            DropIndex("dbo.Scores", new[] { "Course_CourseId" });
            DropTable("dbo.Players");
            DropTable("dbo.Scores");
            DropTable("dbo.Courses");
        }
    }
}
