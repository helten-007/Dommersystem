using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Speaker.ViewModels
{
    // Viewmodel for the judge status panel in the Speaker view
    public class JudgeHasScoredTuple
    {
        public String JudgeName { get; set; }
        public bool HasJudged { get; set; }
        public long JudgeId { get; set; }

        internal static List<JudgeHasScoredTuple> GetJudgeStatusListFor(Tournament t, RoundContestant rc, int runNo)
        {
            var list = new List<JudgeHasScoredTuple>();
            if (rc != null)
            {
                int criteriaCount = t.JudgingCriteria.Count;
                foreach (var judge in t.Judges)
                {
                    var tuplet = new JudgeHasScoredTuple();
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
        internal static List<JudgeHasScoredTuple> GetJudgeStatusListForCurrentHeat(Tournament t)
        {
            var list = new List<JudgeHasScoredTuple>();
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
					var tuplet = new JudgeHasScoredTuple();
					tuplet.JudgeName = judge.Name;
					tuplet.HasJudged = judgementCount == expectedJudgementCount;
					list.Add(tuplet);
				}
            }
            return list;
        }
    }
}