using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    public class ResultsViewModel : _LayoutViewModel
    {
		public Tournament Tournament { get; set; }
        public String TournamentName { get; set; }
        public String RoundTitle { get; set; }
        public int RoundNo { get; set; }
        public int RunCount { get; set; }
        public bool HasNextRound { get; set; }
        public bool HasPreviousRound { get; set; }
        public List<ContestantRoundResultViewModel> Contestants { get; set; }

        public ResultsViewModel(Tournament tourney, int roundNo)
        {
			Tournament = tourney;
            Contestants = new List<ContestantRoundResultViewModel>();
            var round = tourney.GetRoundNoGuarded(roundNo);
            RoundNo = roundNo;
            RunCount = round.RunsPerContestant;
            RoundTitle = round.Title;
            TournamentName = tourney.Name;
            HasNextRound = RoundNo < tourney.Rounds.Count;
            HasPreviousRound = RoundNo > 1;
            if (round.Status != TournamentStatus.Prestart)
            {
                foreach (var contestant in round.GetContestantEntriesScoreSorted())
                {
                    var contestantModel = new ContestantRoundResultViewModel(contestant);
                    Contestants.Add(contestantModel);
                }
            }
        }
    }
}