using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// A contestant's participation in a round of some tournament
    /// </summary>
    public class RoundContestant
    {
        public long Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? TotalScore { get; set; }
        public int HeatNo { get; set; }
        /// <summary>
        /// Position in queue for the given contestant in this round
        /// </summary>
        public int Ordinal { get; set; }

        // Foreign keys / relations
        [Required]
        public virtual Round Round { get; set; }
        [Required]
        public virtual Contestant Contestant { get; set; }
        public virtual ICollection<RunJudging> RunJudgings { get; set; }

        public void EnsureListsAreInitialized()
        {
            if (RunJudgings == null) RunJudgings = new HashSet<RunJudging>();
        }

        /// <summary>
        /// Returns true if the contestant is judged given the count of judges and runs
        /// </summary>
		public bool IsJudged(int expectedJudgementCount)
        {
			return RunJudgings.Count == expectedJudgementCount;
        }

		public bool IsJudged(int expectedJudgementCount, int maxJudgementCount)
		{
			return RunJudgings.Count == expectedJudgementCount || RunJudgings.Count == maxJudgementCount;
		}

        public bool IsJudgedBy(Judge judge, int runNo, int criteriaCount)
        {
            int count = RunJudgings.Count(p => p.RunNo == runNo && p.JudgeId == judge.Id);
            return count == criteriaCount;
        }

        public void ReplaceRunJudging(RunJudging runJudging)
        {
            RunJudgings.Replace(runJudging);
        }

        /// <summary>
        /// Calculates average score if all judges have set their score
        /// </summary>
        /// <param name="expectedJudgmentCount">Number of judge entries expected per run</param>
        public virtual void CalculateTotalScore(int expectedJudgementCountPerRun, int runsPerContestant)
        {
			int expectedJudgementCount = expectedJudgementCountPerRun * runsPerContestant;
			if (IsJudged(expectedJudgementCount, expectedJudgementCountPerRun * runsPerContestant))
            {
                var scoreList = GetRunScores(expectedJudgementCountPerRun, runsPerContestant).Where(p => p.HasValue);
                if (scoreList.Count() > 0)
                {
                    var dummy1 = Round;
                    var dummy2 = Contestant; // Trigger lazy loading or else ... TODO: Find a better solution
                    TotalScore = scoreList.Max(p => p.Value);
                }
            }
        }

		/// <summary>
		/// Use this one instead of the above one. The above one will be removed before release!!!
		/// Only kept the old one because I didn't bother to change all the references in the Test-project.
		/// </summary>
		public virtual void CalculateTotalScore(int expectedJudgementCountPerRun, int roundNo, int runsPerContestant)
		{
			if (roundNo < 1)
				roundNo = 1;
			int expectedJudgementCount = expectedJudgementCountPerRun * roundNo;
			if (IsJudged(expectedJudgementCount, expectedJudgementCountPerRun * runsPerContestant))
			{
				var scoreList = GetRunScores(expectedJudgementCountPerRun, runsPerContestant).Where(p => p.HasValue);
				if (scoreList.Count() > 0)
				{
					var dummy1 = Round;
					var dummy2 = Contestant; // Trigger lazy loading or else ... TODO: Find a better solution
					TotalScore = Math.Round(scoreList.Max(p => p.Value), 2, MidpointRounding.ToEven); //scoreList.Max(p => p.Value);
				}
			}
		}

        /// <summary>
        /// Calculates average score if all judges have set their score
        /// </summary>
        /// <param name="expectedJudgmentCount">Number of judge entries expected per run</param>
        public decimal? GetRunScore(int runNo, int expectedJudgementCount)
        {
            if (expectedJudgementCount <= 0) throw new ArgumentException("expectedJudgementCount must be > 0");
            decimal? score = null;
            var thisRunJudgings = RunJudgings.Where(p => p.RunNo == runNo).ToList();
            if (thisRunJudgings.Count == expectedJudgementCount)
            {
                var nonNullScores = thisRunJudgings.Where(p => p.Score != null).ToList();
                if (nonNullScores.Count > 0)
                {
                    score = nonNullScores.Average(p => p.Score.Value);
                }
            }
            return score;
        }

        /// <summary>
        /// returns average score for judge/run 
        /// </summary>
        public decimal? GetScoreFromJudgeRun(long judgeId, int runNo)
        {
            if (RunJudgings == null) return null;
            var list = RunJudgings.Where(p => p.JudgeId == judgeId && p.RunNo == runNo).ToList();
            if (list.Count > 0) return list.Average(p => p.Score);
            return null;
        }

        /// <summary>
        /// Returns a list of averaged score per run
        /// </summary>
        /// <param name="expectedJudgementCountPerRun">Number of judge entries expected per run</param>
        /// <param name="runsPerContestant">Number of runs per contesstant</param>
        public List<decimal?> GetRunScores(int expectedJudgementCountPerRun, int runsPerContestant)
        {
            var list = new List<decimal?>();
            for (int i = 1; i <= runsPerContestant; i++)
            {
                var score = GetRunScore(i, expectedJudgementCountPerRun);
                list.Add(score);
            }
            return list;
        }

        public override string ToString()
        {
            return String.Format("ID: {0}, TotalScore: {1}, Contestant: {2}", Id, TotalScore, Contestant);
        }
    }
}
