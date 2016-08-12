using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    /// <summary>
    /// Partial view model for ResultsView
    /// </summary>
    public class ContestantRoundResultViewModel
    {
		public int? Position { get; set; }
        public String Name { get; set; }
        public List<decimal?> RunScore { get; set; }
        public decimal? TotalScore { get; set; }

        /// Not allowing this to be called directly since it may result in database access
        public ContestantRoundResultViewModel(RoundContestant rc)
        {
            RunScore = new List<decimal?>();
            Name = rc.Contestant.Name;
            TotalScore = rc.TotalScore;
            if (rc.Round == null || rc.Round.Tournament == null) throw new ArgumentException("Navigation properties Round and Tournament must be set on roundContestant");
            var round = rc.Round;
            var tourney = round.Tournament;
            int expectedJudgementCountPerRun = tourney.GetExpectedJudgementCountPerRun();
            RunScore = rc.GetRunScores(expectedJudgementCountPerRun, round.RunsPerContestant);
        }
    }
}