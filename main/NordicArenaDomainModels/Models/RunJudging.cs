using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Represents a judging of a run by a single a criterion for a single judge
    /// </summary>
    public class RunJudging
    {
        public long Id { get; set; }
        public int RunNo { get; set; }
        /// <summary>
        /// Score for criterion. Null means "opted not to score this criterion".
        /// </summary>
        public decimal? Score { get; set;}

        // Navigation
        [Required]
        public long RoundContestantId { get; set; }
        [Required]
        public long CriterionId { get; set; }
        [Required]
        public long JudgeId { get; set; }
        [JsonIgnore]
        public virtual Judge Judge { get; set; }
        [JsonIgnore]
        public virtual JudgingCriterion Criterion { get; set; }
        [JsonIgnore]
        public virtual RoundContestant RoundContestant { get;set; }

        public RunJudging()
        {
            
        }

        public RunJudging(RoundContestant contestant, Judge judge, JudgingCriterion criterion, int runNo, decimal? score) : this()
        {
            RoundContestant = contestant;
            RoundContestantId = contestant.Id;
            Judge = judge;
            JudgeId = judge.Id;
            Criterion = criterion;
            CriterionId = criterion.Id;
            RunNo = runNo;
            Score = score;
        }

        /// <summary>
        /// True if same criteria, judge, contestant and RunNO
        /// </summary>
        public bool RunEquals(RunJudging other)
        {
            bool equals = RunNo == other.RunNo;
            equals &= JudgeId == other.JudgeId;
            equals &= CriterionId == other.CriterionId;
            equals &= RoundContestantId == other.RoundContestantId;
            return equals;
        }

        public override string ToString()
        {
            return String.Format("RC/Judge/Criteria/Run/Score: {0}/{1}/{2}/{3}/{4}", RoundContestantId, JudgeId, CriterionId, RunNo, Score);
        }
    }

    public static class RunJudgingCollectionExtensions
    {
        // TODO: Remove. Not in use?
        public static void Replace(this ICollection<RunJudging> runJudgings, RunJudging judging)
        {
            var existing = runJudgings.FirstOrDefault(p => p.RunEquals(judging));
            if (existing != null) runJudgings.Remove(existing);
            runJudgings.Add(judging);
        }

        public static void Replace(this IDbSet<RunJudging> runJudgings, RunJudging judging)
        {
            var existing = runJudgings.FirstOrDefault(p =>
                p.RunNo == judging.RunNo &&
                p.JudgeId == judging.JudgeId &&
                p.CriterionId == judging.CriterionId &&
                p.RoundContestantId == judging.RoundContestantId);
            if (existing != null) runJudgings.Remove(existing);
            runJudgings.Add(judging);
        }
    }
}
