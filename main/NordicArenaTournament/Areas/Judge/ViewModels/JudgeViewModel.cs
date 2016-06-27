using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

using NordicArenaTournament.Areas.Speaker.ViewModels;
using NordicArenaTournament.Database;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    /// <summary>
    /// Viewmodel for main judging view for judges
    /// </summary>
    public class JudgeViewModel : _LayoutViewModel
    {
        public Tournament Tournament { get; set; }
        public List<ContestantRunViewModel> Contestants { get; set; }
        public List<JudgingCriterion> Criteria { get; set; }
        public bool CanJudge { get; set; }
        public NordicArenaDomainModels.Models.Judge Judge { get; set; }


		public List<JudgeHasScoredTuple> JudgeStatus { get; set; }


        public JudgeViewModel() { }
        /// <summary>
        /// Instantiates a new JudgeViewModel by loading necessary properties from Tournament..
        /// </summary>
        public JudgeViewModel(Tournament tourney, NordicArenaDomainModels.Models.Judge judge)
        {
			var m = this;
            m.Tournament = tourney;
			m.Criteria = tourney.JudgingCriteria.OrderBy(p => p.Id).ToList();
			m.Judge = judge;
			m.Contestants = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judge.Id);
			m.CanJudge = Contestants.Count > 0 && Contestants[0].Scores.All(p => p.Score == null);
			m.JudgeStatus = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tourney);
        }
    }
}