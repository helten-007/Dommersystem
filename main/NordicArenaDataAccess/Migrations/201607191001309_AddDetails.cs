namespace NordicArenaTournament.Database
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetails : DbMigration
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
                        Stance = c.String(),
                        DateOfBirth = c.DateTime(),
                        IsRemoved = c.Boolean(nullable: false),
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
                        HeatNo = c.Int(nullable: false),
                        Ordinal = c.Int(nullable: false),
                        Round_Id = c.Long(nullable: false),
                        Contestant_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rounds", t => t.Round_Id, cascadeDelete: true)
                .ForeignKey("dbo.Contestants", t => t.Contestant_Id)
                .Index(t => t.Round_Id)
                .Index(t => t.Contestant_Id);
            
            CreateTable(
                "dbo.Rounds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaxContestants = c.Int(nullable: false),
                        RoundNo = c.Int(nullable: false),
                        Title = c.String(),
                        RunsPerContestant = c.Int(nullable: false),
                        SecondsPerRun = c.Int(nullable: false),
                        ContestantsPerHeat = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
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
                        Status = c.Int(nullable: false),
                        CurrentRun = c.String(),
                        IsCurrentRunDone = c.Boolean(nullable: false),
                        ShufflePlayerList = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Judges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LoginCode = c.String(),
                        IsHeadJudge = c.Boolean(nullable: false),
                        TournamentId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tournaments", t => t.TournamentId, cascadeDelete: true)
                .Index(t => t.TournamentId);
            
            CreateTable(
                "dbo.RunJudgings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RunNo = c.Int(nullable: false),
                        Score = c.Decimal(precision: 18, scale: 2),
                        RoundContestantId = c.Long(nullable: false),
                        CriterionId = c.Long(nullable: false),
                        JudgeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JudgingCriterions", t => t.CriterionId)
                .ForeignKey("dbo.Judges", t => t.JudgeId, cascadeDelete: true)
                .ForeignKey("dbo.RoundContestants", t => t.RoundContestantId)
                .Index(t => t.CriterionId)
                .Index(t => t.JudgeId)
                .Index(t => t.RoundContestantId);
            
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
            DropForeignKey("dbo.RoundContestants", "Contestant_Id", "dbo.Contestants");
            DropForeignKey("dbo.RunJudgings", "RoundContestantId", "dbo.RoundContestants");
            DropForeignKey("dbo.RoundContestants", "Round_Id", "dbo.Rounds");
            DropForeignKey("dbo.Rounds", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.Judges", "TournamentId", "dbo.Tournaments");
            DropForeignKey("dbo.RunJudgings", "JudgeId", "dbo.Judges");
            DropForeignKey("dbo.JudgingCriterions", "Tournament_Id", "dbo.Tournaments");
            DropForeignKey("dbo.RunJudgings", "CriterionId", "dbo.JudgingCriterions");
            DropForeignKey("dbo.Rounds", "QualifiesFromRound_Id", "dbo.Rounds");
            DropIndex("dbo.Contestants", new[] { "Tournament_Id" });
            DropIndex("dbo.RoundContestants", new[] { "Contestant_Id" });
            DropIndex("dbo.RunJudgings", new[] { "RoundContestantId" });
            DropIndex("dbo.RoundContestants", new[] { "Round_Id" });
            DropIndex("dbo.Rounds", new[] { "Tournament_Id" });
            DropIndex("dbo.Judges", new[] { "TournamentId" });
            DropIndex("dbo.RunJudgings", new[] { "JudgeId" });
            DropIndex("dbo.JudgingCriterions", new[] { "Tournament_Id" });
            DropIndex("dbo.RunJudgings", new[] { "CriterionId" });
            DropIndex("dbo.Rounds", new[] { "QualifiesFromRound_Id" });
            DropTable("dbo.JudgingCriterions");
            DropTable("dbo.RunJudgings");
            DropTable("dbo.Judges");
            DropTable("dbo.Tournaments");
            DropTable("dbo.Rounds");
            DropTable("dbo.RoundContestants");
            DropTable("dbo.Contestants");
        }
    }
}
