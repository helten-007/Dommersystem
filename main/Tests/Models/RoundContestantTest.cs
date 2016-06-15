using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using Tests.Helpers;

namespace Tests.Models
{
    [TestClass]
    public class RoundContestantTest
    {
        [TestMethod]
        public void IsJudgedTest_blank_no()
        {
            var rc = new RoundContestant();
            rc.EnsureListsAreInitialized();            
            bool result = rc.IsJudged(12);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsJudgedTest_3by2butonly1_false()
        {
            var j1 = new Judge() { Id = 7 };
            var rc = new RoundContestant() { Id = 5 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1 });
            bool result = rc.IsJudged(12);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsJudgedTest_1by2butonly1_true()
        {
            var j1 = new Judge() { Id = 7 };
            var rc = new RoundContestant() { Id = 5 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1 });
            bool result = rc.IsJudged(1);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsJudgedTest_OnlyOneOfTwoCriterion_false()
        {
            var j1 = new Judge() { Id = 4 };
            var j2 = new Judge() { Id = 1 };
            var rc = new RoundContestant() { Id = 7 };
            var crit = new JudgingCriterion() { Id = 65 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j2, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 4, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 5, Judge = j2, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 5, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 1, Judge = j2, Criterion = crit }.SetIdFromObjects());
            bool result = rc.IsJudged(12);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsJudgedTest_3by2by2_true()
        {
            var c1 = new JudgingCriterion() { Id = 7 };
            var c2 = new JudgingCriterion() { Id = 8 };
            var j1 = new Judge() { Id = 1 };
            var j2 = new Judge() { Id = 2 };
            var rc = new RoundContestant() { Id = 10 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j2, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 4, Judge = j1, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 5, Judge = j2, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 5, Judge = j1, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 1, Judge = j2, Criterion = c1 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = c2 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j2, Criterion = c2 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 4, Judge = j1, Criterion = c2 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 5, Judge = j2, Criterion = c2 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 5, Judge = j1, Criterion = c2 }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 1, Judge = j2, Criterion = c2 }.SetIdFromObjects());
            foreach (var rj in rc.RunJudgings) rj.SetIdFromObjects();
            bool result = rc.IsJudged(12);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsJudgedTest_3by2yields6withOneNullScore_true()
        {
            var j1 = new Judge() { Id = 1 };
            var j2 = new Judge() { Id = 2 };
            var rc = new RoundContestant() { Id = 3 };
            var crit = new JudgingCriterion() { Id = 6 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j2, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = 4, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 2, Score = null, Judge = j2, Criterion = crit }.SetIdFromObjects()); // Null score = concious "no score"-mark
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 5, Judge = j1, Criterion = crit }.SetIdFromObjects());
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 3, Score = 1, Judge = j2, Criterion = crit }.SetIdFromObjects());
            foreach (var rj in rc.RunJudgings) rj.SetIdFromObjects();
            bool result = rc.IsJudged(6);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsJudgedBy_Run1Judge1Exists()
        {
            var j1 = new Judge() { Id = 1 };
            var j2 = new Judge() { Id = 2 };
            var rc = new RoundContestant() { Id = 3 };
            var crit = new JudgingCriterion() { Id = 6 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit, JudgeId = j1.Id });

            var result = rc.IsJudgedBy(j1, 1, 1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsJudgedBy_Run2Judge1NotExists()
        {
            var j1 = new Judge() { Id = 1 };
            var j2 = new Judge() { Id = 2 };
            var rc = new RoundContestant() { Id = 3 };
            var crit = new JudgingCriterion() { Id = 6 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit, JudgeId = j1.Id });

            var result = rc.IsJudgedBy(j1, 2, 1);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public void IsJudgedBy_Run1Judge2NotExists()
        {
            var j1 = new Judge() { Id = 1 };
            var j2 = new Judge() { Id = 2 };
            var rc = new RoundContestant() { Id = 3 };
            var crit = new JudgingCriterion() { Id = 6 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit, JudgeId = j1.Id });

            var result = rc.IsJudgedBy(j2, 1, 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsJudgedBy_Run1Judge1MissingCritera()
        {
            var j1 = new Judge() { Id = 1 };
            var rc = new RoundContestant() { Id = 3 };
            var crit = new JudgingCriterion() { Id = 6 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging { RoundContestant = rc, RunNo = 1, Score = 3, Judge = j1, Criterion = crit, JudgeId = j1.Id });

            var result = rc.IsJudgedBy(j1, 1, 2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsJudgedBy_Run1Judge1With2Criteras()
        {
            var j1 = new Judge() { Id = 1 };
            var rc = new RoundContestant() { Id = 3 };
            var crit1 = new JudgingCriterion() { Id = 6 };
            var crit2 = new JudgingCriterion() { Id = 7 };
            rc.EnsureListsAreInitialized();
            rc.ReplaceRunJudging(new RunJudging(rc, j1, crit1, 1, 3.0M));
            rc.ReplaceRunJudging(new RunJudging(rc, j1, crit2, 1, 3.0M));

            var result = rc.IsJudgedBy(j1, 1, 2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ReplaceRunJudging_NewItem_Added()
        {
            var crit1 = new JudgingCriterion() { Id = 5 };
            var crit2 = new JudgingCriterion() { Id = 9 };
            var judge = new Judge() { Id = 8 };
            var rc = new RoundContestant() { Id = 7 };
            rc.EnsureListsAreInitialized();
            var j1 = new RunJudging()
            {
                Id = 1,
                Criterion = crit1,
                Judge = judge,
                Score = 7,
                RoundContestant = rc,
                RunNo = 1
            };

            rc.ReplaceRunJudging(j1);

            Assert.AreEqual(1, rc.RunJudgings.Count);
            Assert.AreSame(j1, rc.RunJudgings.First());
        }

        [TestMethod]
        public void ReplaceRunJudging_EqualItem_Replaced()
        {
            var crit1 = new JudgingCriterion() { Id = 5 };
            var crit2 = new JudgingCriterion() { Id = 9 };
            var judge = new Judge() { Id = 8 };
            var rc = new RoundContestant() { Id = 7 };
            rc.EnsureListsAreInitialized();
            var j1 = new RunJudging()
            {
                Id = 1,
                Criterion = crit1,
                Judge = judge,
                Score = 7,
                RoundContestant = rc,
                RunNo = 1
            };
            var j2 = new RunJudging()
            {
                Id = 5,
                Criterion = crit1,
                Judge = judge,
                Score = 5,
                RoundContestant = rc,
                RunNo = 1
            };

            rc.ReplaceRunJudging(j1);
            rc.ReplaceRunJudging(j2);

            Assert.AreEqual(1, rc.RunJudgings.Count);
            Assert.AreSame(j2, rc.RunJudgings.First());
        }

        [TestMethod]
        public void CalculateTotalScore_4of5_notScored()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 6, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 6, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });

            cont.CalculateTotalScore(5, 1);

            Assert.AreEqual(null, cont.TotalScore);
        }

        [TestMethod]
        public void CalculateTotalScore_5of5butOneNull_ScoredAndCorrectAverage()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 6, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 6, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = null, RunNo = 1 });

            cont.CalculateTotalScore(5, 1);

            Assert.AreEqual(5.5M, cont.TotalScore);

        }

        [TestMethod]
        public void CalculateTotalScore_5of5_Scored()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 2, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 4, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 6, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 9, RunNo = 1 });

            cont.CalculateTotalScore(5, 1);

            Assert.AreEqual(5.8M, cont.TotalScore);
        }

        [TestMethod]
        public void CalculateTotalScore_2of2_Scored()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1 });

            cont.CalculateTotalScore(2, 1);

            Assert.AreEqual(7.5M, cont.TotalScore);
        }

        [TestMethod]
        public void CalculateTotalScore_2runs_BestRunTaken()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2 });

            cont.CalculateTotalScore(2, 2);

            Assert.AreEqual(9M, cont.TotalScore);
        }

        [TestMethod]
        public void GetRunScore_2runsRun1()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2 });

            var score = cont.GetRunScore(1, 2);

            Assert.AreEqual(7.5M, score);
        }

        [TestMethod]
        public void GetRunScore_2runsRun2()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2 });

            var score = cont.GetRunScore(2, 2);

            Assert.AreEqual(9M, score);
        }

        [TestMethod]
        public void GetScoreFromJudge_5Set3Valid1Null_CorrectValue()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = null, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 1, JudgeId = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2, JudgeId = 1 });
            var res = cont.GetScoreFromJudgeRun(1L, 1);
            Assert.AreEqual(7.5M, res);
        }

        [TestMethod]
        public void GetScoreFromJudge_Judge2_CorrectValue()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = null, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 1, JudgeId = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2, JudgeId = 1 });
            var res = cont.GetScoreFromJudgeRun(judgeId: 2L, runNo: 1);
            Assert.AreEqual(8M, res);
        }

        [TestMethod]
        public void GetScoreFromJudge_Run2_CorrectValue()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = 5, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = null, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 1, JudgeId = 1 });
            cont.RunJudgings.Add(new RunJudging() { Score = 8, RunNo = 1, JudgeId = 2 });
            cont.RunJudgings.Add(new RunJudging() { Score = 10, RunNo = 2, JudgeId = 1 });
            var res = cont.GetScoreFromJudgeRun(judgeId: 1L, runNo: 2);
            Assert.AreEqual(10M, res);
        }

        [TestMethod]
        public void GetScoreFromJudge_NoData_Null()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            var res = cont.GetScoreFromJudgeRun(0,0);
            Assert.IsNull(res);
        }

        [TestMethod]
        public void GetRunScore_NullJudgement_NullAvgScore()
        {
            var cont = new RoundContestant();
            cont.EnsureListsAreInitialized();
            cont.RunJudgings.Add(new RunJudging() { Score = null, RunNo = 1});
            var result = cont.GetRunScore(1, 1);
            Assert.IsNull(result);
        }
    }
}
