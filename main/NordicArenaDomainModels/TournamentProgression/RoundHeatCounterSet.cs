using System;
using System.Linq;
using NordicArenaDomainModels.Models;

namespace NordicArenaDomainModels.TournamentProgression
{
    /// <summary>
    /// Counter for Heat:Run. 
    /// </summary>
    public class RoundHeatCounterSet : TournamentCounterSet
    {
        // Making constructor private, as it  may cause DB access. Don't want to give an
        // illusion of it being lightweight.
        private RoundHeatCounterSet(Round round)
        {
            var contestants = round.ContestantEntries.ToList();
            if (!contestants.Any()) throw new InvalidOperationException("Round must have at least one contestant when initializing RoundHeatCounterSet");
            var heatCounter = new Counter(contestants.Max(p => p.HeatNo));
            var runCounter = new Counter(round.RunsPerContestant);
            AddCounter(heatCounter);
            AddCounter(runCounter);
        }

        /// <summary>
        /// Creates a counter set for the round provided. May cause DB access.
        /// </summary>
        public static RoundHeatCounterSet CreateCounterSetFor(Round round) 
        {
            var cs = new RoundHeatCounterSet(round);
            return cs;
        }
    }
}