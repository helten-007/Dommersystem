using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

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


        public JudgeViewModel() { }
        /// <summary>
        /// Instantiates a new JudgeViewModel by loading necessary properties from Tournament..
        /// </summary>
        public JudgeViewModel(Tournament tourney, NordicArenaDomainModels.Models.Judge judge)
        {
            Tournament = tourney;
            Criteria =  tourney.JudgingCriteria.OrderBy(p => p.Id).ToList();
            Judge = judge;
            Contestants = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judge.Id);
            CanJudge = Contestants.Count > 0 && Contestants[0].Scores.All(p => p.Score == null);
        }
    }
}