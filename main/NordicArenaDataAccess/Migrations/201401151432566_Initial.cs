namespace NordicArenaTournament.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contestants",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Sponsors = c.String(),
                        Location = c.String(),
                        Tournament_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.RoundContestants",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TotalScore = c.Decimal(precision: 18, scale: 2),
                        Contestant_Id = c.Long(),
                        Round_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contestants", t => t.Contestant_Id)
                .ForeignKey("dbo.Rounds", t => t.Round_Id)
                .Index(t => t.Contestant_Id)
                .Index(t => t.Round_Id);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaxContestants = c.Int(nullable: false),
                        RoundNo = c.Int(nullable: false),
                        Title = c.String(),
                        QualifiesFromRound_Id = c.Long(),
                        Tournament_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rounds", t => t.QualifiesFromRound_Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.QualifiesFromRound_Id)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.Tournaments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RunsPerContestant = c.Int(nullable: false),
                        SecondsPerRun = c.Int(nullable: false),
                        ContestantsPerHeat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Judges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LoginCode = c.String(),
                        Tournament_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.Tournament_Id);
            
            CreateTable(
                "dbo.JudgingCriterions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Min = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Max = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Step = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tournament_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.Tournament_Id, cascadeDelete: true)
                .Index(t => t.Tournament_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contestants", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Rounds", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.JudgingCriterions", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Judges", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Rounds", "QualifiesFromRound_Id", "dbo.Rounds");
            DropForeignKey("dbo.RoundContestants", "Round_Id", "dbo.Rounds");
            DropForeignKey("dbo.RoundContestants", "Contestant_Id", "dbo.Contestants");
            DropIndex("dbo.Contestants", new[] { "Tournament_Id" });
            DropIndex("dbo.Rounds", new[] { "Tournament_Id" });
            DropIndex("dbo.JudgingCriterions", new[] { "Tournament_Id" });
            DropIndex("dbo.Judges", new[] { "Tournament_Id" });
            DropIndex("dbo.Rounds", new[] { "QualifiesFromRound_Id" });
            DropIndex("dbo.RoundContestants", new[] { "Round_Id" });
            DropIndex("dbo.RoundContestants", new[] { "Contestant_Id" });
            DropTable("dbo.JudgingCriterions");
            DropTable("dbo.Judges");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Rounds");
            DropTable("dbo.RoundContestants");
            DropTable("dbo.Contestants");
        }
    }
}
