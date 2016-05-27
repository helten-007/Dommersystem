using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;

namespace NordicArenaDomainModels.TournamentProgression
{
    /// <summary>
    /// Counter for Heat:Run:ContestantNo. 
    /// </summary>
    public class RoundIndividualCounterSet : TournamentCounterSet
    {
        List<RoundContestant> _contestants;

        // Making constructor private, as it  may cause DB access. Don't want to give an
        // illusion of it being lightweight.
        private RoundIndividualCounterSet(Round round)
        {
            _contestants = round.ContestantEntries.ToList(); 
            var heatCounter = new Counter(_contestants.Max(p => p.HeatNo));
            var runCounter = new Counter(round.RunsPerContestant);
            var contestantCounter = new Counter(() => {return GetContestantCountInHeat(heatCounter);});
            AddCounter(heatCounter);
            AddCounter(runCounter);
            AddCounter(contestantCounter);
        }

        /// <summary>
        /// Creates a counter set for the round provided. May cause DB access.
        /// </summary>
        public static RoundIndividualCounterSet CreateCounterSetFor(Round round) 
        {
            var cs = new RoundIndividualCounterSet(round);
            return cs;
        }

        public int GetContestantCountInHeat(Counter counterForHeat)
        {
            var heatNo = counterForHeat.Value;
            var contestantCountInHeat = _contestants.Count(p => p.HeatNo == heatNo);
            return contestantCountInHeat;
        }

        internal int GetContesantNo()
        {
            return Counters[2].Value;
        }
    }
}