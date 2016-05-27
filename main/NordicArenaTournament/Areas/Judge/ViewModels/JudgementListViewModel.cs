using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Judge.ViewModels
{
    /// <summary>
    /// List of judgements passed for all contestants in a given round
    /// </summary>
    public class JudgementListViewModel : _LayoutViewModel
    {
        public long TournamentId { get; set; }
        public String TournamentName { get; set; }
        public String RoundTitle { get; set; }
        public int RoundNo { get; set; }
        public bool HasNextRound { get; set; }
        public bool HasPreviousRound { get; set; }
        public List<RunJudgementViewModel> Judgements { get; set; }
        public List<JudgingCriterion> Criteria { get; set; }


        public JudgementListViewModel()
        {
            Judgements = new List<RunJudgementViewModel>();
            Criteria = new List<JudgingCriterion>();
        }

        public JudgementListViewModel(Tournament tournament, int roundNo) : this()
        {
            var round = tournament.GetRoundNoGuarded(roundNo);
            RoundNo = roundNo;
            TournamentName = tournament.Name;
            TournamentId = tournament.Id;
            RoundTitle = round.Title;
            HasNextRound = RoundNo < tournament.Rounds.Count;
            HasPreviousRound = RoundNo > 1;
            Criteria = tournament.JudgingCriteria.OrderBy(p => p.Id).ToList();
            // Todo: Profile and Optimize. This generates X number of selects on RoundContestant and Y number of selects on Contestant table.
            var allRunJudgings = round.ContestantEntries.SelectMany(p => p.RunJudgings).OrderBy(p => p.JudgeId).ToList();
            var roundContestantIds = round.ContestantEntries.Select(p => p.Id).Distinct().ToList();
            var judgeIds = tournament.Judges.Select(p => p.Id).ToList();
            foreach (var judgeId in judgeIds)
            {
                var judge = tournament.Judges.FirstOrDefault(p => p.Id == judgeId);
                foreach (var rcId in roundContestantIds)
                {
                    var rc = round.ContestantEntries.FirstOrDefault(p => p.Id == rcId);
                    for (int runNo = 1; runNo <= round.RunsPerContestant; runNo++)
                    {
                        var scoreList = allRunJudgings.Where(p => p.RunNo == runNo && p.JudgeId == judgeId && p.RoundContestantId == rcId).OrderBy(p => p.CriterionId).ToList();
                        var modelLine = new RunJudgementViewModel(scoreList, judge, rc, runNo);
                        Judgements.Add(modelLine);
                    }
                }
            }
        }

        /// <summary>
        /// Converts list of Judgements to runjudgings
        /// </summary>
        public List<RunJudging> GetRunJudgings(Tournament tournament)
        {
            var criteria = tournament.JudgingCriteria.OrderBy(p => p.Id).ToList();
            var runJudgings = Judgements.SelectMany(p => p.GetAsRunJudgings(criteria)).ToList();
            return runJudgings;
        } 
    }
}