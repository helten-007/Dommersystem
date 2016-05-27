using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Judge.Controllers;
using NordicArenaTournament.Areas.Judge.ViewModels;
using NordicArenaTournament.SignalR;
using Tests.Helpers;

namespace Tests.Controllers
{
    [TestClass]
    public class TournamentJudgeControllerTest
    {
        [TestMethod]
        public void ReplaceRunJudgings_OneEmptyScoringFirstOfTwoJudges_NoProb()
        {
            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            var target = new TournamentJudgeController(null, db, new Mock<NaHub>().Object);


            var tourney = Factory.CreateStartedTournament(2, 1, 2).WithJudgingCriteria(2).WithJudges(2);
            db.AddTournament(tourney);
            
            var model = new JudgeViewModel();
            model.Tournament = tourney;
            model.Contestants = new List<ContestantRunViewModel>();
            
            // Setup input
            var contestants = tourney.GetRoundNo(1).ContestantEntries.ToList();
            var c1 = new ContestantRunViewModel();
            c1.RoundContestantId = contestants[0].Id;
            c1.TournamentId = tourney.Id;
            c1.Scores = new List<RunJudging>()
            {
                new RunJudging() { JudgeId = 1, CriterionId = 1, RoundContestantId = c1.RoundContestantId, Score = 5.5M },
                new RunJudging() { JudgeId = 1, CriterionId = 2, RoundContestantId = c1.RoundContestantId, Score = 6.5M }
            };
            var c2 = new ContestantRunViewModel();
            c2.RoundContestantId = contestants[1].Id;
            c2.TournamentId = tourney.Id;
            c2.Scores = new List<RunJudging>()
            {
                new RunJudging() { JudgeId = 1, CriterionId = 1, RoundContestantId = c2.RoundContestantId, Score = null },
                new RunJudging() { JudgeId = 1, CriterionId = 2, RoundContestantId = c2.RoundContestantId, Score = null }
            };
            model.Contestants.Add(c1);
            model.Contestants.Add(c2);
            // Setup GetRoundContestant to return from tourney-bound object
            dbMock.Setup(p => p.GetRoundContestantGuarded(It.IsAny<long>()))
                .Returns<long>(id => contestants.FirstOrDefault(p => p.Id == id));
            foreach (var rj in c1.Scores) contestants[0].RunJudgings.Add(rj); // This will normally be taken care of by Enitity Framework. Not so in mocked context.

            // Act & Assert
            target.JudgeIndex(model);

            Assert.AreEqual(null, contestants[0].TotalScore);
            Assert.AreEqual(null, contestants[1].TotalScore);
        }


        [TestMethod]
        public void ReplaceRunJudgings_OneEmptyScoringLastOfTwoJudges_NoException()
        {
            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            var target = new TournamentJudgeController(null, db, new Mock<NaHub>().Object);
            var tourney = Factory.CreateStartedTournament(2, 1, 2).WithJudgingCriteria(2).WithJudges(2);
            db.AddTournament(tourney);

            var model = new JudgeViewModel();
            model.Tournament = tourney;
            model.Contestants = new List<ContestantRunViewModel>();

            // Setup input
            var contestants = tourney.GetRoundNo(1).ContestantEntries.ToList();
            var c1 = new ContestantRunViewModel();
            c1.RoundContestantId = contestants[0].Id;
            c1.TournamentId = tourney.Id;
            c1.Scores = new List<RunJudging>()
            {
                new RunJudging() { JudgeId = 1, CriterionId = 1, RoundContestantId = c1.RoundContestantId, Score = null },
                new RunJudging() { JudgeId = 1, CriterionId = 2, RoundContestantId = c1.RoundContestantId, Score = null },
                new RunJudging() { JudgeId = 2, CriterionId = 1, RoundContestantId = c1.RoundContestantId, Score = null },
                new RunJudging() { JudgeId = 2, CriterionId = 2, RoundContestantId = c1.RoundContestantId, Score = null }
            };
            model.Contestants.Add(c1);
            // Setup GetRoundContestant to return from tourney-bound object
            dbMock.Setup(p => p.GetRoundContestantGuarded(It.IsAny<long>()))
                .Returns<long>(id => contestants.FirstOrDefault(p => p.Id == id));
            foreach (var rj in c1.Scores) contestants[0].RunJudgings.Add(rj); // This will normally be taken care of by Enitity Framework. Not so in mocked context.

            // Act & Assert
            target.JudgeIndex(model);
        }
    }
}
