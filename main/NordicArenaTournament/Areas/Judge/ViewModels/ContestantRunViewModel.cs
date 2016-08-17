using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    /// <summary>
    /// Partial viewmodel for JudgeList: Represents a run for a contestant
    /// </summary>
    public class ContestantRunViewModel
    {
        public long TournamentId { get; set; }
        public int RunNo { get; set; }
        public string ContestantName { get; set; }
		public RoundContestant Contestant { get; set; }
		public string Stance { get; set; }
        public long RoundContestantId { get; set; }
        public List<RunJudging> Scores { get; set; }

        public ContestantRunViewModel()
        {
            Scores = new List<RunJudging>();
        }

        /// <summary>
        /// Returns a list of Contestants with scores currently running in the competition specified
        /// </summary>
        /// <param name="tourney"></param>
        /// <param name="judgeId"></param>
        /// <returns></returns>
        public ContestantRunViewModel(Tournament tourney, Round round, long judgeId, long roundContestantId) : this()
        {
            var counter = tourney.GetRoundCounter();
			Contestant = round.GetRoundContestantGuarded(roundContestantId);
            TournamentId = tourney.Id;
            RunNo = counter.GetRunNo();
            RoundContestantId = roundContestantId;
			ContestantName = Contestant.Contestant.Name;
			Stance = Contestant.Contestant.Stance;
			var scores = Contestant.RunJudgings
                .Where(p => p.RunNo == RunNo && p.JudgeId == judgeId)
                .OrderBy(p => p.CriterionId)
                .ToList();
            Scores = MapScoresToCriteria(tourney.JudgingCriteria, scores, judgeId, RunNo, roundContestantId);
        }

        private static List<RunJudging> MapScoresToCriteria(IEnumerable<JudgingCriterion> criteria, List<RunJudging> scores, long judgeId, int runNo, long rcId)
        {
            var criteriaIds = criteria.OrderBy(p => p.Id).Select(p => p.Id);
            var list = new List<RunJudging>();
            foreach (var critId in criteriaIds)
            {
                var scoreForCrit = scores.FirstOrDefault(p => p.CriterionId == critId);
                if (scoreForCrit == null) // Create score object with 
                {
                    scoreForCrit = new RunJudging
                    {
                        CriterionId = critId,
                        JudgeId = judgeId,
                        RunNo = runNo,
                        RoundContestantId = rcId
                    };
                }
                list.Add(scoreForCrit);
            }
            return list;
        }

        /// <summary>
        /// Retruns list of contestants currently running in the current heat, with stored scores (if any) for the specified judge
        /// </summary>s
        /// <param name="tourney">Tournament ID</param>
        /// <param name="judgeId">Judge ID</param>
        public static List<ContestantRunViewModel> CreateListOfCurrentConestants(Tournament tourney, long judgeId)
        {
            var round = tourney.GetCurrentRound();
            if (round == null) throw new ArgumentException("No active round");
            var currentHeat = tourney.GetRoundCounter().GetHeatNo();
            var contestants = round.GetContestantsInHeat(currentHeat).ToList();
            var modelList = new List<ContestantRunViewModel>();
            foreach (long contestantId in contestants.Select(p => p.Id))
            {
                var model = new ContestantRunViewModel(tourney, round, judgeId, contestantId);
                modelList.Add(model);
            }
            return modelList;
        }
    }
}

