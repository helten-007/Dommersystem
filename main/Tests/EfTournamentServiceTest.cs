using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Judge.ViewModels;
using Tests.Helpers;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class EfTournamentServiceTest
    {
        [TestMethod]
        public void DeleteJudge_Existing_Ok()
        {
            var context = DatabaseFaker.GetFake().Object;
            context.Judges.Add(new Judge() { Id = 1 } );
            context.DeleteJudge(1);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void DeleteJudge_NonExistant_Exception()
        {
            var context = DatabaseFaker.GetFake().Object;
            context.DeleteJudge(1);
        }

        [TestMethod]
        public void ReplaceRunJudgings()
        {
            // Setup input
            var model = new ContestantRunViewModel();
            model.RoundContestantId = 16;
            model.TournamentId = 136;
            model.Scores = new System.Collections.Generic.List<RunJudging>()
            {
                new RunJudging() { JudgeId = 1, CriterionId = 1, RoundContestantId = model.RoundContestantId, Score = 5.5M },
                new RunJudging() { JudgeId = 1, CriterionId = 2, RoundContestantId = model.RoundContestantId, Score = 6.5M }
            };
            // Setup mocks
            var expectedJudgementCountPerRun = 3;
            var tournamentMock = new Mock<Tournament>();
            var round = new Round() { RunsPerContestant = 7 };
            var rcMock = new Mock<RoundContestant>();
            rcMock.Setup(p => p.Round).Returns(round);
            var contextMock = DatabaseFaker.GetFake();
            contextMock.CallBase = true;
            contextMock.Setup(p => p.GetRoundContestantGuarded(model.RoundContestantId)).Returns(rcMock.Object);
            contextMock.Setup(p => p.GetTournamentGuarded(model.TournamentId)).Returns(tournamentMock.Object);
            tournamentMock.Setup(p => p.GetExpectedJudgementCountPerRun()).Returns(expectedJudgementCountPerRun);

            // Act & Assert
            contextMock.Object.ReplaceRunJudgings(model.TournamentId, model.RoundContestantId, model.Scores);

			rcMock.Verify(p => p.CalculateTotalScore(expectedJudgementCountPerRun, round.RunsPerContestant));
            Assert.AreEqual(2, contextMock.Object.RunJudgings.Count());
        }
    }
}
