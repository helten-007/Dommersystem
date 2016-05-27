using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;

namespace Tests.Models
{
    [TestClass]
    public class RunJudgingTest
    {
        [TestMethod]
        public void RunEquals_AllButIdAndScoreEqual_true()
        {
            var crit = new JudgingCriterion() { Id = 5 };
            var judge = new Judge() { Id = 8 };
            var rc = new RoundContestant() { Id = 7 };

            var j1 = new RunJudging(rc, judge, crit, 1, 7M);
            var j2 = new RunJudging(rc, judge, crit, 1, 6M);
            j1.Id = 1;
            j2.Id = 2;

            var result = j1.RunEquals(j2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RunEquals_JudgeDiffers_false()
        {
            var crit = new JudgingCriterion() { Id = 5 };
            var judge1 = new Judge() { Id = 8 };
            var judge2 = new Judge() { Id = 13 };
            var rc = new RoundContestant() { Id = 7 };

            var j1 = new RunJudging(rc, judge1, crit, 1, 7M);
            var j2 = new RunJudging(rc, judge2, crit, 1, 6M);
            j1.Id = 1;
            j2.Id = 2;

            var result = j1.RunEquals(j2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RunEquals_RoundContestantDiffers_False()
        {
            var crit = new JudgingCriterion() { Id = 5 };
            var judge = new Judge() { Id = 8 };
            var rc1 = new RoundContestant() { Id = 77 };
            var rc2 = new RoundContestant() { Id = 71 };


            var j1 = new RunJudging(rc1, judge, crit, 1, 7M);
            var j2 = new RunJudging(rc2, judge, crit, 1, 6M);
            j1.Id = 1;
            j2.Id = 2;

            var result = j1.RunEquals(j2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RunEquals_RunNoDiffers_false()
        {
            var crit = new JudgingCriterion() { Id = 5 };
            var judge = new Judge() { Id = 8 };
            var rc = new RoundContestant() { Id = 7 };

            var j1 = new RunJudging(rc, judge, crit, 1, 7M);
            var j2 = new RunJudging(rc, judge, crit, 2, 6M);
            j1.Id = 1;
            j2.Id = 2;

            var result = j1.RunEquals(j2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RunEquals_CriterionDiffers_false()
        {
            var crit1 = new JudgingCriterion() { Id = 5 };
            var crit2 = new JudgingCriterion() { Id = 9 };
            var judge = new Judge() { Id = 8 };
            var rc = new RoundContestant() { Id = 7 };

            var j1 = new RunJudging(rc, judge, crit1, 1, 7M);
            var j2 = new RunJudging(rc, judge, crit2, 1, 6M);
            j1.Id = 1;
            j2.Id = 2;

            var result = j1.RunEquals(j2);

            Assert.IsFalse(result);
        }
    }
}
