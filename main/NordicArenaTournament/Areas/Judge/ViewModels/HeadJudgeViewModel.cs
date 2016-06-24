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
    public class HeadJudgeViewModel : _LayoutViewModel
    {
        public Tournament Tournament { get; set; }
        public List<ContestantRunViewModel> Contestants { get; set; }
        public List<JudgingCriterion> Criteria { get; set; }
        public bool CanJudge { get; set; }
		public ICollection<NordicArenaDomainModels.Models.Judge> Judges { get; set; }
		public NordicArenaDomainModels.Models.Judge Judge { get; set; }
		public List<JudgeHasScoredTuple> JudgeStatus { get; set; }
		public List<JudgeViewModel> JudgeViewModels { get; set; }
		public List<decimal?> AverageCriteriaScore { get; set; }

        public HeadJudgeViewModel() { }

        /// <summary>
        /// Instantiates a new HeadJudgeViewModel by loading necessary properties from Tournament..
        /// </summary>
		public HeadJudgeViewModel(Tournament tourney, NordicArenaDomainModels.Models.Judge judge, ICollection<NordicArenaDomainModels.Models.Judge> judges)
        {
            Tournament = tourney;
            Criteria =  tourney.JudgingCriteria.OrderBy(p => p.Id).ToList();
            Judges = judges;
			Judge = judge;
            Contestants = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judge.Id);
			//CanJudge = Contestants.Count > 0 && Contestants[0].Scores.All(p => p.Score != null);
			JudgeStatus = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tourney);
			JudgeViewModels = GetJudgeViewModels();

			AverageCriteriaScore = new List<decimal?>();

			foreach (var j in JudgeStatus)
				if (!j.HasJudged)
					CanJudge = false;

			if (CanJudge)
				SetAverageScores();
        }

		private List<JudgeViewModel> GetJudgeViewModels()
		{
			var judgeViewModels = new List<JudgeViewModel>();
			var judges = Judges.Where(p => p.Tournament.Id == Tournament.Id && p.IsHeadJudge == false)
                .ToList();

			foreach (var j in judges)
				judgeViewModels.Add(new JudgeViewModel(Tournament, j));
			return judgeViewModels;
		}

		private void SetAverageScores()
		{
			decimal? score = 0.0m;
			var judgeCount = JudgeViewModels.Count();
			var contestantCount = JudgeViewModels[0].Contestants.Count();
			var critCount = JudgeViewModels[0].Contestants[0].Scores.Count();

			var counter = 0;


			for (var i = 0; i < contestantCount; i++)
			{
				for (var j = 0; j < critCount; j++)
				{
					for (var k = 0; k < judgeCount; k++)
					{
						var temp = JudgeViewModels[k].Contestants[i].Scores[j].Score;
						if (temp != null)
						{
							score += temp;
							counter++;
						}
					}
					if (counter > 0)
						AverageCriteriaScore.Add(score / counter);
					else
						AverageCriteriaScore.Add(0.0m);
					counter = 0;
					score = 0;
				}
			}
		}
    }
}