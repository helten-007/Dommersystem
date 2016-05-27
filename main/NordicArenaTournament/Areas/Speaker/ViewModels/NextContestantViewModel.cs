using System;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Speaker.ViewModels
{
    public class NextContestantViewModel
    {
        public String Name { get; set; }
        public decimal? TotalScore { get; set; }
        public NextContestantViewModel(RoundContestant rc)
        {
            Name = rc.Contestant.Name;
            TotalScore = rc.TotalScore;
        }
    }
}