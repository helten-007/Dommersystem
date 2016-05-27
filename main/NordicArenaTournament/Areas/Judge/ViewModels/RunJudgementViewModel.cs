using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;
using Judge = NordicArenaDomainModels.Models.Judge;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    /// <summary>
    /// Partial view model for JudgementList-view
    /// </summary>
    public class RunJudgementViewModel
    {
        public long JudgeId { get; set; }
        public long RoundContestantId { get; set; }
        public String ContestantName { get; set; }
        public String JudgeName { get; set; }
        public int RunNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? RunJudgeScore { get; set; }
        [DisplayFormat(DataFormatString = "{0:N1}", ApplyFormatInEditMode = true)]
        public List<decimal?> Scores { get; set; }

        public RunJudgementViewModel()
        {
            Scores = new List<decimal?>();
        }

        public RunJudgementViewModel(List<RunJudging> runJudgings, NordicArenaDomainModels.Models.Judge judge = null, RoundContestant roundContestant = null, int? runNo = null)
            : this()
        {
            if ((roundContestant == null || runNo == null || judge == null) && (runJudgings == null || runJudgings.Count == 0)) 
                throw new ArgumentException("No entries in list or list null, while not supplied judge, roundContestant and runNo explicitly");
            if (roundContestant == null) roundContestant = runJudgings[0].RoundContestant;
            if (runNo == null) runNo = runJudgings[0].RunNo;
            if (judge == null) judge = runJudgings[0].Judge;

            ContestantName = roundContestant.Contestant != null ? roundContestant.Contestant.Name : "(not set)";
            RunNo = runNo.Value;
            JudgeName = judge.Name;
            RoundContestantId = roundContestant.Id;
            JudgeId = judge.Id;
            RunJudgeScore = roundContestant.GetScoreFromJudgeRun(judge.Id, runNo.Value);
            foreach (var rj in runJudgings)
            {
                if (rj.RunNo != runNo) throw new ArgumentException(String.Format("Expected all runJudgements to be from run {0}, but one is from run {1}", runNo, rj.RunNo));
                if (rj.RoundContestantId != roundContestant.Id) throw new ArgumentException(String.Format("Expected all runJudgements to be from roundContestantId {0}, but one is from {1}", roundContestant.Id, rj.RoundContestantId));
                if (rj.JudgeId != judge.Id) throw new ArgumentException(String.Format("Expected all runJudgements to be from judgeId {0}, but one is from {1}", judge.Id, rj.JudgeId));
                Scores.Add(rj.Score);
            }
        }

        /// <summary>
        /// Converts list back to a list of RunJudgings -mapping it to the criteria list supplied
        /// </summary>
        /// <param name="orderedCriteriaList">ID-ordered list of Criterias, mapping to the scores by inddex</param>
        /// <returns>List of runJudgings</returns>
        public List<RunJudging> GetAsRunJudgings(List<JudgingCriterion> orderedCriteriaList)
        {
            var list = new List<RunJudging>();
            if (!orderedCriteriaList.AreInRisingOrder(p => p.Id)) throw new ArgumentException("Criteria list must be ordered by ID because the score list is");
            if (Scores.Count == 0) return list;
            if (orderedCriteriaList.Count != Scores.Count) throw new ArgumentException("You must supply as many criterias as there are scores");
            for (int i = 0; i < Scores.Count; i++)
            {
                var rj = new RunJudging()
                {
                    RoundContestantId = RoundContestantId,
                    JudgeId = JudgeId,
                    RunNo = RunNo,
                    CriterionId = orderedCriteriaList[i].Id,
                    Score = Scores[i]
                };
                list.Add(rj);
            }
            return list;
        }
    }
}