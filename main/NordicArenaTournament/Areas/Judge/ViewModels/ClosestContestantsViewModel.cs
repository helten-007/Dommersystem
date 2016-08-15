using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;
using NordicArenaTournament.Areas.Admin.ViewModels;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
	public class ClosestContestantsViewModel
	{
        public List<ContestantRoundResultViewModel> Contestants { get; set; }

        public ClosestContestantsViewModel(Tournament tourney, int roundNo)
        {
            Contestants = new List<ContestantRoundResultViewModel>();
            var round = tourney.GetRoundNoGuarded(roundNo);

            if (round.Status != TournamentStatus.Prestart)
            {
				int index = 1;
                foreach (var contestant in round.GetContestantEntriesScoreSorted())
                {
					System.Diagnostics.Debug.WriteLine("index: " + index);
                    var contestantModel = new ContestantRoundResultViewModel(contestant);
					contestantModel.Position = index;
                    Contestants.Add(contestantModel);
					index++;
                }
            }
        }
	}
}