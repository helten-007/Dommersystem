using System.Data.Entity;
using NordicArenaDomainModels.Models;

// Warning: Fixing namespace will cause trouble with the migration history
namespace NordicArenaTournament.Database
{
    public class NordicArenaDataContext : DbContext
    {
        public IDbSet<Tournament> Tournaments { get; set; }
        public IDbSet<Judge> Judges { get; set; }
        public IDbSet<Contestant> Contestants { get; set; }
        public IDbSet<RoundContestant> RoundContestants { get; set; }
        public IDbSet<Round> Rounds { get; set; }
        public IDbSet<RunJudging> RunJudgings { get; set; }
        public IDbSet<JudgingCriterion> Criteria { get; set; }
		public IDbSet<User> Users { get; set; }

        public NordicArenaDataContext() : base("name=Data")
        {                
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Eliminating cascade delete on one of the paths to RunJudgings to avoid SQL Server complaining
            modelBuilder.Entity<JudgingCriterion>().HasMany(p => p.RunJudgings).WithRequired(p => p.Criterion).WillCascadeOnDelete(false); 
            modelBuilder.Entity<Contestant>().HasMany<RoundContestant>(p => p.RoundParticipations).WithRequired(p => p.Contestant).WillCascadeOnDelete(false);
            modelBuilder.Entity<RoundContestant>().HasMany<RunJudging>(p => p.RunJudgings).WithRequired(p => p.RoundContestant).WillCascadeOnDelete(false);
        }
    }
}