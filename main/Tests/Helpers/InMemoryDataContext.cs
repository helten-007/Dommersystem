using System;
using NordicArenaDomainModels.Models;
using NordicArenaServices;

namespace Tests.Helpers
{
    public class InMemoryDataContext : EfTournamentService
    {
        public bool IsDisposed { get; set; }
        public InMemoryDataContext()
        {
            Tournaments = new ListSet<Tournament>();
            Contestants = new ListSet<Contestant>();
            Judges = new ListSet<Judge>();
            Rounds = new ListSet<Round>();
            RunJudgings = new ListSet<RunJudging>();
            RoundContestants = new ListSet<RoundContestant>();
            Criteria = new ListSet<JudgingCriterion>();
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }

        public new void SaveChanges()
        {
            if (IsDisposed) throw new InvalidOperationException("Cannot save a Disposed DbContext-object");
        }
    }
}
