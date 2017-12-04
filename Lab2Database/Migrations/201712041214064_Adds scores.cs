namespace Lab2Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsscores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        ScoreId = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        MapId = c.Int(nullable: false),
                        Moves = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScoreId)
                .ForeignKey("dbo.Maps", t => t.MapId, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId)
                .Index(t => t.MapId);
            
            AlterColumn("dbo.Maps", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Players", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "PlayerId", "dbo.Players");
            DropForeignKey("dbo.Scores", "MapId", "dbo.Maps");
            DropIndex("dbo.Scores", new[] { "MapId" });
            DropIndex("dbo.Scores", new[] { "PlayerId" });
            AlterColumn("dbo.Players", "Name", c => c.String());
            AlterColumn("dbo.Maps", "Name", c => c.String());
            DropTable("dbo.Scores");
        }
    }
}
