using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Judge.ViewModels;
using Tests.Helpers;

namespace Tests.ViewModels
{
    [TestClass]
    public class ContestantRunViewModelTest
    {
        [TestMethod]
        public void ContestantRunViewModelCreate_Heat1_ContestantNames()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2) // results in 2 heats 
                .WithJudges(2)
                .WithJudgingCriteria(2);

            var target = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, 1);

            Assert.AreEqual(2, target.Count, "2 Contestants");
            Assert.AreEqual("C1", target[0].ContestantName);
            Assert.AreEqual("C2", target[1].ContestantName);
        }

        [TestMethod]
        public void ContestantRunViewModelCreate_Heat2_ContestantNames()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2) // results in 2 heats 
                .WithJudges(2)
                .WithJudgingCriteria(2);
            tourney.GetRoundCounter().GetHeatCounter().Value = 2;

            var target = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, 1);

            Assert.AreEqual(2, target.Count, "2 Contestants");
            Assert.AreEqual("C3", target[0].ContestantName);
            Assert.AreEqual("C4", target[1].ContestantName);
        }

        [TestMethod]
        public void ContestantRunViewModelCreate_Judge1Contestant1_Scores()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2) // results in 2 heats 
                .WithJudges(2)
                .WithJudgingCriteria(2);

            // Adding a bunch of judgings. Some which should be returned, some which should be filtered out
            var rc1 = tourney.GetCurrentRound().GetRoundContestantGuarded(1);
            var rc2 = tourney.GetCurrentRound().GetRoundContestantGuarded(2);
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 1, Score = 641 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 2, CriterionId = 1, Score = 12 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 67 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 2, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 765 });
            rc2.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc2.Id, RunNo = 1, CriterionId = 3, Score = 777 });

            var target = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judgeId: 1);

            Assert.AreEqual(2, target.Count, "2 Contestants");
            Assert.AreEqual(2, target[0].Scores.Count, "2 scores");
            Assert.AreEqual(641, target[0].Scores[0].Score);
            Assert.AreEqual(67, target[0].Scores[1].Score);
        }

        [TestMethod]
        public void ContestantRunViewModelCreate_Judge1Contestant2_Scores()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2) // results in 2 heats 
                .WithJudges(2)
                .WithJudgingCriteria(2);

            // Adding a bunch of judgings. Some which should be returned, some which should be filtered out
            var rc1 = tourney.GetCurrentRound().GetRoundContestantGuarded(1);
            var rc2 = tourney.GetCurrentRound().GetRoundContestantGuarded(2);
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 1, Score = 641 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 2, CriterionId = 1, Score = 12 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 67 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 2, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 765 });
            rc2.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc2.Id, RunNo = 1, CriterionId = 3, Score = 777 });

            var target = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judgeId: 2);

            Assert.AreEqual(2, target.Count, "2 Contestants");
            Assert.AreEqual(2, target[0].Scores.Count, "scorecount");
            Assert.AreEqual(2, target[1].Scores.Count, "0 scores for contestant 1");
            Assert.AreEqual(null, target[0].Scores[0].Score);
            Assert.AreEqual(765, target[0].Scores[1].Score);
        }

        [TestMethod]
        public void ContestantRunViewModelCreate_Judge2BothContestants_Scores()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2) // results in 2 heats 
                .WithJudges(2)
                .WithJudgingCriteria(3);

            // Adding a bunch of judgings. Some which should be returned, some which should be filtered out
            var rc1 = tourney.GetCurrentRound().GetRoundContestantGuarded(1);
            var rc2 = tourney.GetCurrentRound().GetRoundContestantGuarded(2);
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 1, Score = 641 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 2, CriterionId = 1, Score = 12 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 67 });
            rc1.RunJudgings.Add(new RunJudging { JudgeId = 2, RoundContestantId = rc1.Id, RunNo = 1, CriterionId = 2, Score = 765 });
            rc2.RunJudgings.Add(new RunJudging { JudgeId = 1, RoundContestantId = rc2.Id, RunNo = 1, CriterionId = 3, Score = 777 });

            var target = ContestantRunViewModel.CreateListOfCurrentConestants(tourney, judgeId: 1);

            Assert.AreEqual(2, target.Count, "2 Contestants");
            Assert.AreEqual(3, target[1].Scores.Count, "scoreCount");
            // Expecting all scores objects to be set, but some to have Score=null. cshtml file depends on this
            Assert.AreEqual(null, target[1].Scores[0].Score);
            Assert.AreEqual(null, target[1].Scores[1].Score);
            Assert.AreEqual(777, target[1].Scores[2].Score);
        }
    }
}
