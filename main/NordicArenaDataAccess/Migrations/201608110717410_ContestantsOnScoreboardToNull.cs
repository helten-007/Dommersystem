namespace NordicArenaTournament.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContestantsOnScoreboardToNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tournaments", "ContestantsOnScoreboard", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tournaments", "ContestantsOnScoreboard", c => c.Int(nullable: false));
        }
    }
}
