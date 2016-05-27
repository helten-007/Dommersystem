using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaServices;
using Tests.Helpers;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class EfTournamentServiceIntegrationTest
    {
        [ClassInitialize]
        public static void Preconditions(TestContext morradi)
        {
            var tourney = Factory.CreateInitializedTourney();
            if (tourney.Rounds.Count != 1)
                throw new Exception(
                    "These tests are written with expectation of every tournament initializing with 1 round");
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateTournamentAndRound_AddRound()
        {
            var setupService = new EfTournamentService();
            var actService = new EfTournamentService();
            var assertService = new EfTournamentService();

            var tourney = Factory.CreateInitializedTourney();
            setupService.AddTournament(tourney); 
            long id = tourney.Id;
            try
            {
                var newRoundList = new List<Round>();
                var initialRound = new Round("Q");
                newRoundList.Add(initialRound);
                newRoundList.Add(tourney.Rounds.First());
                tourney.Rounds = newRoundList;
                
                // Act
                actService.UpdateTournamentAndRounds(tourney);

                var result = assertService.GetTournamentGuarded(id);
                var roundList = result.GetRoundsOrdered();
                Assert.AreEqual("Q", roundList.First().Title);
                Assert.AreEqual(newRoundList[1].Title, roundList.Last().Title);
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateTournamentAndRound_DeleteRound()
        {
            var setupService = new EfTournamentService();
            var actService = new EfTournamentService();
            var assertService = new EfTournamentService();

            var tourney = Factory.CreateInitializedTourney();
            var round = new Round("Superfinale");
            round.RoundNo = 2;
            round.QualifiesFromRound = tourney.GetRoundNo(1);
            tourney.Rounds.Add(round);
            setupService.AddTournament(tourney);
            long id = tourney.Id;
            try
            {
                var newRoundList = new List<Round>();
                newRoundList.Add(tourney.GetRoundNo(2));
                tourney.Rounds = newRoundList;

                // Act
                actService.UpdateTournamentAndRounds(tourney);

                var result = assertService.GetTournamentGuarded(id);
                var roundList = result.GetRoundsOrdered().ToList();
                Assert.AreEqual(1, roundList.Count);
                Assert.AreEqual("Superfinale", roundList[0].Title);
                Assert.AreEqual(null, roundList[0].QualifiesFromRound);
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void UpdateTournamentAndRound_UpdateRoundAndTournamentTitle()
        {
            var setupService = new EfTournamentService();
            var actService = new EfTournamentService();
            var assertService = new EfTournamentService();

            var tourney = Factory.CreateInitializedTourney();
            setupService.AddTournament(tourney);
            long id = tourney.Id;
            try
            {

                // Act
                tourney.GetRoundNo(1).Title = "Roger";
                tourney.Name = "Franz";
                actService.UpdateTournamentAndRounds(tourney);

                var result = assertService.GetTournamentGuarded(id);
                Assert.AreEqual("Franz", result.Name);
                Assert.AreEqual("Roger", result.GetRoundNo(1).Title);
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void CreateTournament_IsCommitedBeforeOutOfScope()
        {
            long? tourneyId = null;
            try
            {
                var service1 = new EfTournamentService();
                string tourneyName = "Integration test tournament" + Guid.NewGuid().ToString();
                var tournament = new Tournament(tourneyName);
                service1.AddTournament(tournament);
                Assert.IsTrue(tournament.Id != 0, "Tournament ID not set");
                tourneyId = tournament.Id;
                // Create a new context to verfiy with certainty that the data is committed to the database
                var service2 = new EfTournamentService();
                var result = service2.GetTournamentGuarded(tournament.Id);
                Assert.AreEqual(tourneyName, result.Name);
            }
            finally 
            {
                DatabaseHelper.DeleteTournament(tourneyId);
            }
        }
    }
}
