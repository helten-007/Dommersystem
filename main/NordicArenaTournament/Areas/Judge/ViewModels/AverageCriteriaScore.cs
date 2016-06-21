using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    // Viewmodel for the judge status panel in the Speaker view
    public class AverageCriteriaScore
    {
        public String JudgeName { get; set; }
        public bool HasJudged { get; set; }
        public long JudgeId { get; set; }
		
		/**
		 * Finn en måte å fylle en liste med gjennomsnittsverdier av de forskjellige kriteriene
		 * fra alle dommerene. Disse skal oppdateres automatisk.
		 */

		//public List<decimal?> AverageCriteriaScores { get; set; }

        internal static List<AverageCriteriaScore> GetJudgeStatusListFor(Tournament t, RoundContestant rc, int runNo)
        {
            var list = new List<AverageCriteriaScore>();
            if (rc != null)
            {
                int criteriaCount = t.JudgingCriteria.Count;
                foreach (var judge in t.Judges)
                {
                    var tuplet = new AverageCriteriaScore();
                    tuplet.JudgeName = judge.Name;
                    tuplet.HasJudged = rc.IsJudgedBy(judge, runNo, criteriaCount);
                    list.Add(tuplet);
                }
            }
            return list;

        }

        /// <summary>
        ///  Returns a list of JudgeHasScoredTuple which indicates whether or not the judges are done judging this heat
        /// </summary>
        internal static List<AverageCriteriaScore> GetJudgeStatusListForCurrentHeat(Tournament t)
        {
            var list = new List<AverageCriteriaScore>();
            var round = t.GetCurrentRound();
            if (round == null) return list;
            var heatNo = t.GetRoundCounter().GetHeatNo();
            var runNo = t.GetRoundCounter().GetRunNo();
            var heatContestants = round.GetContestantsInHeat(heatNo);
            int criteriaCount = t.JudgingCriteria.Count;
            int expectedJudgementCount = criteriaCount * heatContestants.Count();
            var contestantIds = heatContestants.Select(p => p.Id).ToList();
            foreach (var judge in t.Judges)
            {
				if (!judge.IsHeadJudge)
				{
					var judgementCount = judge.RunJudgings.Count(p => p.RunNo == runNo && contestantIds.Contains(p.RoundContestantId));
					var tuplet = new AverageCriteriaScore();
					tuplet.JudgeName = judge.Name;
					tuplet.HasJudged = judgementCount == expectedJudgementCount;
					list.Add(tuplet);
				}
            }
            return list;
        }
    }
}