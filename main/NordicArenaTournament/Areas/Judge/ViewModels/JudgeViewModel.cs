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


		public ICollection<NordicArenaDomainModels.Models.Judge> Judges { get; set; }
		public List<JudgeViewModel> JudgeViewModels { get; set; }
		public List<decimal?> AverageCriteriaScore { get; set; }
		public List<JudgeHasScoredTuple> JudgeStatus { get; set; }
		public bool HasHeadJudgeJudged { get; set; }


        public JudgeViewModel() { }
        /// <summary>
        /// Instantiates a new JudgeViewModel by loading necessary properties from Tournament..
        /// </summary>
        public JudgeViewModel(Tournament tourney, NordicArenaDomainModels.Models.Judge judge)
        {
            Tournament = tourney;
			Criteria = tourney.JudgingCriteria.OrderBy(p => p.Id).ToList();
			Judge = judge;
			Contestants = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judge.Id);

			if (Judge.IsHeadJudge)
			{
				AverageCriteriaScore = new List<decimal?>();
				RemoveHeadJudgeFromJudgeList(Tournament.Judges, Judge.Id);
				Judges = Tournament.Judges;

				JudgeStatus = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tourney);
				JudgeViewModels = GetJudgeViewModels();
				CanJudge = true;
				foreach (var j in JudgeStatus)
					if (!j.HasJudged)
						CanJudge = false;

				HasHeadJudgeJudged = SetHasHeadJudgeJudged();
				if (!HasHeadJudgeJudged)
					SetAverageScores();
			}
			else
				CanJudge = Contestants.Count > 0 && Contestants[0].Scores.All(p => p.Score == null);
        }

		private bool SetHasHeadJudgeJudged()
		{
			foreach (var runJ in Judge.RunJudgings)
				if (runJ.Score != null)
					return true;
			return false;
		}

		private void RemoveHeadJudgeFromJudgeList(ICollection<NordicArenaDomainModels.Models.Judge> judges, long? judgeId)
		{
			foreach (var j in judges)
			{
				if (j.Id == judgeId)
				{
					judges.Remove(j);
					break;
				}
			}
		}

		private List<JudgeViewModel> GetJudgeViewModels()
		{
			var judgeViewModels = new List<JudgeViewModel>();
			var judges = Judges.Where(p => p.Tournament.Id == Tournament.Id && p.IsHeadJudge == false).ToList();

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
						AverageCriteriaScore.Add(null/*0.0m*/);
					counter = 0;
					score = 0;
				}
			}
		}
    }
}