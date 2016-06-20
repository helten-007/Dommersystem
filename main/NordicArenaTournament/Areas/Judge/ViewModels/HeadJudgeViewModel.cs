using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;
using NordicArenaTournament.Areas.Speaker.ViewModels;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    /// <summary>
    /// Viewmodel for main judging view for judges
    /// </summary>
    public class HeadJudgeViewModel : _LayoutViewModel
    {
        public Tournament Tournament { get; set; }
        public List<ContestantRunViewModel> Contestants { get; set; }
        public List<JudgingCriterion> Criteria { get; set; }
        public bool CanJudge { get; set; }
		public ICollection<NordicArenaDomainModels.Models.Judge> Judges { get; set; }
		public List<JudgeHasScoredTuple> JudgeStatus { get; set; }


        public HeadJudgeViewModel() { }

        /// <summary>
        /// Instantiates a new HeadJudgeViewModel by loading necessary properties from Tournament..
        /// </summary>
        public HeadJudgeViewModel(Tournament tourney, ICollection<NordicArenaDomainModels.Models.Judge> judges)
        {
            Tournament = tourney;
            Criteria =  tourney.JudgingCriteria.OrderBy(p => p.Id).ToList();
            Judges = judges;
            Contestants = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judge.Id);
            CanJudge = Contestants.Count > 0 && Contestants[0].Scores.All(p => p.Score == null);

			if (round.Status == TournamentStatus.Running)
			{
				m.JudgeStatus = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tournament);
			}
        }
    }
}