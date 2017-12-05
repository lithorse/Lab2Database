namespace Lab2Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MovestoMovesLeft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Scores", "MovesLeft", c => c.Int(nullable: false));
            DropColumn("dbo.Scores", "Moves");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scores", "Moves", c => c.Int(nullable: false));
            DropColumn("dbo.Scores", "MovesLeft");
        }
    }
}
