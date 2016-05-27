using NordicArenaDomainModels.Models;
using NordicArenaDomainModels.TournamentProgression;
using Tests.Helpers;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Tests.Models
{
    // Testing the tournament class when configured with RoundIndividualCounterSet
    public partial class TournamentTest
    {
        [TestMethod]
        public void NextRun_1Plus2()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.First()));
            tournament.GetRoundCounter().SetValue(1, 1, 1);

            tournament.NextRun();
            tournament.NextRun();

            Assert.AreEqual(1, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(2, tournament.GetCurrentRunNo());
        }

        [TestMethod]
        public void NextRun_1Plus3()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.First()));
            tournament.GetRoundCounter().SetValue(1, 1, 1);

            tournament.NextRun();
            tournament.NextRun();
            tournament.NextRun();

            Assert.AreEqual(2, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(2, tournament.GetCurrentRunNo());
        }

        [TestMethod]
        public void NextRun_Plus4TwoPlayersTwoRuns_AtEndOfRound()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.First()));
            tournament.GetRoundCounter().SetValue(1, 1, 1);

            Exception occuredException = null;
            tournament.NextRun();
            tournament.NextRun();
            tournament.NextRun();
            try
            {
                tournament.NextRun();
            }
            catch (Exception ex) { occuredException = ex; }
            Assert.IsNotNull(occuredException);
            Assert.AreEqual(2, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(2, tournament.GetCurrentRunNo());
            Assert.AreEqual(TournamentStatus.Running, tournament.Rounds.FirstOrDefault().Status);
            Assert.AreEqual(TournamentStatus.Running, tournament.Status);

        }

        [TestMethod]
        public void PreviousRun_AtRun2Player2()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.First()));
            tournament.GetRoundCounter().SetValue(1, 2, 2);
            tournament.PreviousRun();

            Assert.AreEqual(1, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(2, tournament.GetCurrentRunNo());
        }

        [TestMethod]
        public void PreviousRun_AtRun1Player2()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.First()));
            tournament.GetRoundCounter().SetValue(1, 1, 2);
            tournament.PreviousRun();

            Assert.AreEqual(1, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(1, tournament.GetCurrentRunNo());
        }

        [TestMethod]
        public void NextRun_1To2_SecondContestantFirstRun()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.SetRoundCounter(RoundIndividualCounterSet.CreateCounterSetFor(tournament.Rounds.FirstOrDefault()));

            tournament.NextRun();

            Assert.AreEqual(2, tournament.GetCurrentRoundContestant().Ordinal);
            Assert.AreEqual(1, tournament.GetCurrentRunNo());
        } 
    }
}
