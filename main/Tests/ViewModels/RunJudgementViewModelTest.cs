using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Judge.ViewModels;

namespace Tests.ViewModels
{
    [TestClass]
    public class RunJudgementViewModelTest
    {
        [TestMethod]
        public void GetAsRunJudgings_2Scores_MappedCorrectly()
        {
            var inList = new List<RunJudging>();
            var crit1 = new JudgingCriterion() { Id = 1, Title = "A" };
            var crit2 = new JudgingCriterion() { Id = 2, Title = "B" };
            var rc = new RoundContestant{ Id = 6 };
            var judge = new Judge {Id = 15};
            inList.Add(new RunJudging(rc, judge, crit1, 1, 7.5M)); 
            inList.Add(new RunJudging(rc, judge, crit2, 1, 5.5M));
            var target = new RunJudgementViewModel(inList);
            
            var result = target.GetAsRunJudgings(new List<JudgingCriterion>() {crit1, crit2});

            Assert.IsTrue(result[0].RunEquals(inList[0]), "0");
            Assert.IsTrue(result[1].RunEquals(inList[1]), "1");
        }
    }
}
