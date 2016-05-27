using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Admin.ViewModels;
using Tests.Helpers;

namespace Tests.ViewModels
{
    [TestClass]
    public class ResultsViewModelTest
    {
        [TestCleanup]
        public void Cleanup()
        {
            ServiceFaker.ResetIocContainer();
        }

        [TestMethod]
        public void ResultsViewModelCreate_RoundNotStarted_NoException()
        {
            var tourney = Factory.CreateInitializedTourney();
            var result = new ResultsViewModel(tourney, 1);
            Assert.AreEqual(0, result.Contestants.Count);
        }

        [TestMethod]
        public void ResultsViewModelCreate_RoundStartedButNotScored_NoException()
        {
            var tourney = Factory.CreateStartedTournament(2, 2, 1).WithJudges(1);
            var result = new ResultsViewModel(tourney, 1);
            Assert.AreEqual(2, result.Contestants.Count);
        }

        [TestMethod]
        public void ResultsViewModelCreate_CorrectNameAndRoundNo()
        {
            var tourney = Factory.CreateStartedTournament(2, 2, 1).WithJudges(1);
            tourney.Name = "NM";
            var result = new ResultsViewModel(tourney, 1);
            Assert.AreEqual(1, result.RoundNo);
            Assert.AreEqual(2, result.RunCount);
            Assert.AreEqual("NM", tourney.Name);
        }

        [TestMethod]
        public void ResultsViewModelCreate_2runs2contestantsAtRound1_CorrectContent()
        {
            var tourney = Factory.CreateStartedTournament(2, 2, 1).WithJudgingCriteria(1).WithJudges(1);
            var round = tourney.GetRoundNo(1);
            var contestants = round.GetContestantEntriesOrdered();
            contestants[0].RunJudgings.Add(new RunJudging { RunNo = 1, RoundContestantId = contestants[0].Id, Score = 5M });
            contestants[0].RunJudgings.Add(new RunJudging { RunNo = 2, RoundContestantId = contestants[0].Id, Score = 6M });
            contestants[1].RunJudgings.Add(new RunJudging { RunNo = 1, RoundContestantId = contestants[1].Id, Score = 8M });
            contestants[1].RunJudgings.Add(new RunJudging { RunNo = 2, RoundContestantId = contestants[1].Id, Score = 10M });
            contestants[0].TotalScore = 5.6M; // Intentially not setting it to the correct total value, to verify that model fetches directly from table
            contestants[1].TotalScore = 9.0M;
            
            ResultsViewModel result = new ResultsViewModel(tourney, 1);

            // Expect contestants to be sorted by highest score first, all data populated
            Assert.AreEqual(2, result.Contestants.Count);
            Assert.AreEqual("C2", result.Contestants[0].Name);
            Assert.AreEqual(8.0M, result.Contestants[0].RunScore[0]);
            Assert.AreEqual(10.0M, result.Contestants[0].RunScore[1]);
            Assert.AreEqual(9.0M, result.Contestants[0].TotalScore);
            Assert.AreEqual("C1", result.Contestants[1].Name);
            Assert.AreEqual(5.0M, result.Contestants[1].RunScore[0]);
            Assert.AreEqual(6.0M, result.Contestants[1].RunScore[1]);
            Assert.AreEqual(5.6M, result.Contestants[1].TotalScore);
        }
    }
}
