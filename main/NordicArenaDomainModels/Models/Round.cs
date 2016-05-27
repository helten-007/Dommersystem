using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.ObjectValidation;
using NordicArenaDomainModels.Resources;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Paramters for a round in a tournament
    /// </summary>
    public class Round
    {
        public long Id { get; set; }
        [Range(0, 10000)]
        public int MaxContestants { get; set; }
        public int RoundNo { get; set; }
        public String Title { get; set; }
        [Range(1, 100)]
        public int RunsPerContestant { get; set; }
        [Range(1, 100000)]
        public int SecondsPerRun { get; set; }
        [Range(1, 10000)]
        public int ContestantsPerHeat { get; set; }
        public TournamentStatus Status { get; set; }

        // Navigation
        public virtual Round QualifiesFromRound { get; set; }
        public virtual ICollection<RoundContestant> ContestantEntries { get; set; }
        [Required]
        public virtual Tournament Tournament { get; set; }

        public Round()
        {
            RunsPerContestant = 1;
            ContestantsPerHeat = 1;
            SecondsPerRun = 60;
        }
        public Round(string title) : this()
        {
            Title = title;
        }

        public void EnsureListsAreInitialized()
        {
            if (ContestantEntries == null) ContestantEntries = new HashSet<RoundContestant>();
        }

        public List<Object> EraseExistingContestantsAndJudgings()
        {
            var deletedObjects = new List<Object>();
            foreach (var rc in ContestantEntries)
            {
                rc.EnsureListsAreInitialized();
                deletedObjects.AddRange(rc.RunJudgings.ToList());
                rc.RunJudgings.Clear();
            }
            deletedObjects.AddRange(ContestantEntries);
            ContestantEntries.Clear();
            return deletedObjects;
        }

        public List<RoundContestant> GetContestantEntriesOrdered()
        {
            return ContestantEntries.OrderBy(p => p.Ordinal).ToList();
        }

        public ResultReasonTuple CanBeEnded()
        {
            var result = new ResultReasonTuple(true);
            if (Status == TournamentStatus.Prestart) return new ResultReasonTuple(false, Text.RoundNotStarted);
            if (Status == TournamentStatus.Ended) return new ResultReasonTuple(false, Text.RoundAlreadyEnded);
            var nonScoredContestants = ContestantEntries.Where(p => p.TotalScore == null).Select(p => p.Contestant.Name);
            if (nonScoredContestants.Count() != 0)
            {
                result.IsTrue = false;
                if (nonScoredContestants.Count() > 1) result.Reason = Text.ContestantsNotScored;
                else result.Reason = String.Format(Text.ContestantXNotScored, nonScoredContestants.First());
            }
            return result;
        }

        /// <summary>
        /// Ends the round, locks the scores
        /// </summary>
        public void End()
        {
            var canBeEnded = CanBeEnded();
            if (!canBeEnded.IsTrue) throw new InvalidOperationException(canBeEnded.Reason);
            Status = TournamentStatus.Ended;
        }

        /// <summary>
        /// Starts the round and assigns contestants to heats
        /// </summary>
        /// <param name="listSorter">A class providing method for sorting the list by a custom criteria</param>
        /// <param name="contestants">List of contestants to add to the round. If not provided, will look for results in previous round and find those who qualify</param>
        public void Start(IListSorter listSorter, IEnumerable<Contestant> contestants = null)
        {
            // Make sure the list is blank before we start. We may be restarting an earlier ran round
            EnsureListsAreInitialized();
            EraseExistingContestantsAndJudgings();
            // (re)generate list of contestants
            if (contestants == null)
            {
                if (QualifiesFromRound == null) throw new ArgumentException("Either contestants must be specified, or QualifiesFromRound must be set");
                contestants = QualifiesFromRound.GetContestantEntriesScoreSorted().Select(p => p.Contestant);
            }

            // Cut out the number of playes who are to qualify
            if (MaxContestants > 0) contestants = contestants.Take(MaxContestants);
            int count = MaxContestants;
            if (count == 0) count = Int32.MaxValue;
            count = Math.Min(contestants.Count(), count);

            // Define heats
            List<Contestant> contestantList = contestants.ToList();
            if (ContestantsPerHeat == 0) ContestantsPerHeat = contestantList.Count;

            listSorter.Sort(contestantList); // Sort list by some custom criteria
            for (int i = 0; i < count; i++) 
            {
                var rc = AddContesant(contestantList[i]);
                rc.HeatNo = i / ContestantsPerHeat + 1;
                rc.Ordinal = i + 1;
            }
            Status = TournamentStatus.Running;              
        }

        /// <summary>
        /// Resets the state of this round. Oppsite of Start()
        /// </summary>
        /// <returns>List of deleted objects</returns>
        public List<Object> Reset()
        {
            EnsureListsAreInitialized();
            var deletedObjects = EraseExistingContestantsAndJudgings();
            Status = TournamentStatus.Prestart;
            return deletedObjects;
        }

        public RoundContestant AddContesant(Contestant contestant)
        {
            contestant.EnsureListsAreInitialized();
            var rc = new RoundContestant()
            {
                Contestant = contestant,
                Round = this
            };
            contestant.RoundParticipations.Add(rc);
            this.ContestantEntries.Add(rc);
            return rc;
        }

        public IOrderedEnumerable<RoundContestant> GetContestantEntriesScoreSorted()
        {
            return ContestantEntries.OrderByDescending(p => p.TotalScore);
        }

        public IOrderedEnumerable<RoundContestant> GetContestantsInHeat(int heatNo)
        {
            return ContestantEntries.Where(p => p.HeatNo == heatNo).OrderBy(p => p.Ordinal);
        }

        public RoundContestant GetRoundContestantGuarded(long roundContestantId)
        {
            var contestant = ContestantEntries.FirstOrDefault(p => p.Id == roundContestantId);
            if (contestant == null) throw new ArgumentException(String.Format("Could not find contestant with ID {0} in round ID {1}", roundContestantId, Id));
            return contestant;
        }

        public void AssignHeatFor(RoundContestant rc)
        {
            var contestants = ContestantEntries.ToList();
            var maxHeat = contestants.Max(p => p.HeatNo);
            int ordinal = contestants.Max(p => p.Ordinal) + 1;
            var playersInHeat = contestants.Count(p => p.HeatNo == maxHeat);
            if (playersInHeat >= ContestantsPerHeat) maxHeat++;
            rc.HeatNo = maxHeat;
            rc.Ordinal = ordinal;
        }
    }
}