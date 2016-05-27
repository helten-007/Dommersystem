using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NordicArenaDomainModels.Helpers;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Models;
using Tests.Common;
using Tests.Helpers;

namespace Tests.Models
{
    [TestClass]
    public class RoundTest
    {
        [TestMethod]
        public void RoundCtor_Status_PreStart()
        {
            var round = new Round();
            Assert.AreEqual(TournamentStatus.Prestart, round.Status);
        }

        [TestMethod]
        public void CanEndRound_RoundNotStarted_False()
        {
            var round = new Round().WithContestants(2, true);
            round.Status = TournamentStatus.Prestart;

            var result = round.CanBeEnded();

            Assert.IsFalse(result.IsTrue);
            Assert.IsNotNull(result.Reason);
        }

        [TestMethod]
        public void CanEndRound_RoundEnded_False()
        {
            var round = new Round().WithContestants(2, true);
            round.Status = TournamentStatus.Ended;

            var result = round.CanBeEnded();

            Assert.IsFalse(result.IsTrue);
            Assert.IsNotNull(result.Reason);
        }

        [TestMethod]
        public void CanEndRound_RoundStartedMoreThanOneContestantLeft_False()
        {
            var round = new Round().WithContestants(2, false);
            round.Status = TournamentStatus.Running;

            var result = round.CanBeEnded();

            Assert.IsFalse(result.IsTrue);
            Assert.IsNotNull(result.Reason);
        }

        [TestMethod]
        public void CanEndRound_RoundStartedOneContestantLeft_FalseAndNameMentioned()
        {
            var round = new Round().WithContestants(3, true);
            round.Status = TournamentStatus.Running;
            var contestant = round.ContestantEntries.LastOrDefault();
            contestant.Contestant.Name = "Roger";
            contestant.TotalScore = null;

            var result = round.CanBeEnded();

            Assert.IsFalse(result.IsTrue);
            Assert.IsTrue(result.Reason.Contains("Roger"));
        }

        [TestMethod]
        public void CanEndRound_RoundStartedAllDone_True()
        {
            var round = new Round().WithContestants(3, true);
            round.Status = TournamentStatus.Running;

            var result = round.CanBeEnded();

            Assert.IsTrue(result.IsTrue);
            Assert.IsNull(result.Reason);
        }

        [TestMethod]
        public void GetContestantEntriesScoreSorted_3Guys()
        {
            var round = new Round().WithContestants(3, false);
            var contestants = round.ContestantEntries.ToList();
            contestants[0].TotalScore = 2;
            contestants[1].TotalScore = 1;
            contestants[2].TotalScore = 3;

            var sorted = round.GetContestantEntriesScoreSorted().ToList();

            Assert.AreSame(contestants[2], sorted[0]);
            Assert.AreSame(contestants[0], sorted[1]);
            Assert.AreSame(contestants[1], sorted[2]);
        }

        [TestMethod]
        public void Start_RandomizedListSorter_SomewhatRandom()
        {
            var listSorter = new RandomizedListSorter();
            var round = new Round();
            var contestants = new List<Contestant>();
            contestants.Add(new Contestant("1"));
            contestants.Add(new Contestant("2"));
            contestants.Add(new Contestant("3"));
            contestants.Add(new Contestant("4"));
            var backup = new List<Contestant>(contestants);
            round.Start(listSorter, contestants);
            // Create a copy of the first shuffled list to act as a baseline
            var firstOrder = new List<Contestant>(round.ContestantEntries.Select(p => p.Contestant));
            int i = 0;
            int maxAttempts = 50;
            for (; i < maxAttempts; i++)
            {
                Thread.Sleep(10); // Random function depends on clock to get seed.
                contestants = new List<Contestant>(backup); // Clone the backup just to be sure we have an identical initial list
                round.Start(listSorter, contestants);
                var result = new List<Contestant>(round.ContestantEntries.Select(p => p.Contestant));
                for (int j = 0; j < result.Count; j++)
                {
                    if (result[j] != firstOrder[j]) goto success; // We have a difference. Shuffling occurs
                }
            }
            success:
            Assert.IsFalse(i == maxAttempts, "We got the same result in all " + maxAttempts + " attempts. Are you sure this is random?");
        }
    }
}
