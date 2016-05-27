using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Common
{
    /// <summary>
    /// For debugging/testing purposes
    /// </summary>
    public class TestDataFactory
    {
        /// <summary>
        /// Used to quickly assign a score to all contestants in a round. 
        /// </summary>
        /// <param name="tourney"></param>
        /// <param name="round"></param>
        public static List<RunJudging> ScoreAllContestants(Tournament tourney, Round round)
        {
            // Kvadruppel-nøsted loop. Ikke hver dag du ser det, eller hva? 
            var rand = new Random();
            var scoreList = new List<RunJudging>();
            foreach (var contestant in round.ContestantEntries)
            {
                for (int runNo = 1; runNo < round.RunsPerContestant; runNo++)
                {
                    foreach (var crit in tourney.JudgingCriteria)
                    {
                        foreach (var judge in tourney.Judges)
                        {
                            var judgement = new RunJudging();
                            judgement.CriterionId = crit.Id;
                            judgement.JudgeId = judge.Id;
                            judgement.RoundContestantId = contestant.Id;
                            judgement.RunNo = runNo;
                            judgement.Score = (decimal) ((decimal) rand.NextDouble() * (crit.Max - crit.Min) +  crit.Min);
                            contestant.ReplaceRunJudging(judgement);
                            scoreList.Add(judgement);
                        }
                    }
                }
            }
            return scoreList;
        }
    }
}