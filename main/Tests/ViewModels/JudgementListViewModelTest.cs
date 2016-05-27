using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Judge.ViewModels;
using Tests.Helpers;

namespace Tests.ViewModels
{
    [TestClass]
    public class JudgementListViewModelTest
    {
        [TestCleanup]
        public virtual void CleanUp()
        {
            ServiceFaker.ResetIocContainer();
        }

        [TestMethod]
        public void JudgementListViewModelCreate_16judgementsWith2Criterias_8Lines()
        {

            var dbMock = DatabaseFaker.GetFake();
            var tourney = Factory.CreateStartedTournament(2, 2, 1)
                .WithJudges(2)
                .WithJudgingCriteria(2)
                .WithJudgementsForRound(1);
            dbMock.Object.FeedWith(tourney);

            var model = new JudgementListViewModel(tourney, 1);

            Assert.AreEqual(8, model.Judgements.Count);
        }

        [TestMethod]
        public void JudgementListViewModelCreate_16judgementsWith2Criterias_2ScoresPerLine()
        {
            var db = DatabaseFaker.GetFake().Object;
            var tourney = Factory.CreateStartedTournament(2, 2, 1)
                .WithJudges(2)
                .WithJudgingCriteria(2)
                .WithJudgementsForRound(1);
            db.FeedWith(tourney);

            var model = new JudgementListViewModel(tourney, 1);

            var scoreList = model.Judgements.Select(p => p.Scores);
            Assert.IsTrue(scoreList.All(p => p.Count == 2));
        }

        [TestMethod]
        public void JudgementListViewModelCreate_16judgementsWith2Criterias_ScoresOrderedAsCriterias()
        {
            var db = DatabaseFaker.GetFake().Object;
            var tourney = Factory.CreateStartedTournament(1, 1, 1)
                .WithJudges(1)
                .WithJudgingCriteria(5); // Precondition: Judging criterias are generated with ID's in rising order
            if (!tourney.JudgingCriteria.AreInRisingOrder(p => p.Id)) throw new InvalidOperationException("Expected elements to be in rising order");
            var cont = tourney.Rounds.First().ContestantEntries.First();
            var crits = tourney.JudgingCriteria.ToList();
            var judge = tourney.Judges.First();
            // Add the scores in random order
            cont.RunJudgings.Add(new RunJudging(cont, judge, crits[2], 1, 5.0M));
            cont.RunJudgings.Add(new RunJudging(cont, judge, crits[1], 1, 6.0M));
            cont.RunJudgings.Add(new RunJudging(cont, judge, crits[4], 1, 7.0M));
            cont.RunJudgings.Add(new RunJudging(cont, judge, crits[3], 1, 8.0M));
            cont.RunJudgings.Add(new RunJudging(cont, judge, crits[0], 1, 9.0M));
            db.FeedWith(tourney);

            var result = new JudgementListViewModel(tourney, 1);

            // Assert that criterias are ordered by CriteriaId
            Assert.AreEqual(crits[0], result.Criteria[0]);
            Assert.AreEqual(crits[1], result.Criteria[1]);
            Assert.AreEqual(crits[2], result.Criteria[2]);
            Assert.AreEqual(crits[3], result.Criteria[3]);
            Assert.AreEqual(crits[4], result.Criteria[4]);
            // Assert that scores are ordered by CriteriaId
            var scores = result.Judgements[0].Scores;
            Assert.AreEqual(9.0M, scores[0]);
            Assert.AreEqual(6.0M, scores[1]);
            Assert.AreEqual(5.0M, scores[2]);
            Assert.AreEqual(8.0M, scores[3]);
            Assert.AreEqual(7.0M, scores[4]);
        }

        [TestMethod]
        public void JudgementListViewModelCreate_16judgementsWith2Criterias_GroupByJudgeBeforeContestant()
        {
            var db = DatabaseFaker.GetFake().Object;
            var tourney = Factory.CreateStartedTournament(2, 2, 1)
                .WithJudges(2)
                .WithJudgingCriteria(2)
                .WithJudgementsForRound(1);
            db.FeedWith(tourney);

            var model = new JudgementListViewModel(tourney, 1);

            Assert.AreEqual("J1", model.Judgements[0].JudgeName);
            Assert.AreEqual("J1", model.Judgements[1].JudgeName);
            Assert.AreEqual(model.Judgements[0].ContestantName, model.Judgements[1].ContestantName);
            Assert.AreEqual(1, model.Judgements[0].RunNo);
            Assert.AreEqual(2, model.Judgements[1].RunNo);
            Assert.AreEqual("J1", model.Judgements[2].JudgeName);
        }

        [TestMethod]
        public void JudgementListViewModelCreate_NoJudgements_AllStillListed()
        {
            var db = DatabaseFaker.GetFake().Object;
            var tourney = Factory.CreateStartedTournament(2, 2, 1)
                .WithJudges(2)
                .WithJudgingCriteria(2);
            db.FeedWith(tourney);

            var model = new JudgementListViewModel(tourney, 1);

            Assert.AreEqual(8, model.Judgements.Count);
        }
    }
}
