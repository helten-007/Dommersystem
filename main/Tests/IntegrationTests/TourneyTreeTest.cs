using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaServices;
using NordicArenaTournament.Database;
using Tests.Helpers;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class TourneyTreeTest
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void IT_TournamentShellCreate()
        {
            long? id = null;
            try
            {
                // Setup/Act
                using (var db = new NordicArenaDataContext())
                {
                    var tourney = new Tournament("I-Test tournament");
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();
                    id = tourney.Id;
                }
                // Asssert
                using (var db = new EfTournamentService())
                {
                    var tourney = db.GetTournament(id.Value);
                    Assert.AreEqual("I-Test tournament", tourney.Name);
                }
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void IT_FullTournamentCreate()
        {
            long? id = null;
            try
            {
                // Setup part 1
                using (var db = new NordicArenaDataContext())
                {
                    var tourney = new Tournament("I-Test tournament");
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();
                    id = tourney.Id;
                }
                // Setup part 2
                using (var db = new EfTournamentService())
                {
                    // Add a first round and add the contestant to that round
                    var tourney = db.GetTournament(id.Value);
                    var contestant1 = new Contestant() { Name = "Roger" };
                    var contestant2 = new Contestant() { Name = "Hans" };
                    tourney.InitializeDefaults();
                    tourney.Contestants.Add(contestant1);
                    tourney.Contestants.Add(contestant2);
                    db.SaveChanges();
                }
                // Setup part 3
                using (var db = new EfTournamentService())
                {
                    var tourney = db.GetTournament(id.Value);
                    
                    var contestant = tourney.Contestants.First(p => p.Name == "Hans");
                    var round = tourney.Rounds.First();
                    tourney.AddContesantToRound(contestant, round);
                    db.SaveChanges();
                }
                // Asssert
                using (var db = new EfTournamentService())
                {
                    var tourney = db.GetTournament(id.Value);
                    Assert.AreEqual("I-Test tournament", tourney.Name);
                    Assert.AreEqual(1, tourney.Rounds.Count);
                    Assert.AreEqual(2, tourney.Contestants.Count);
                    Assert.AreEqual(1, tourney.Rounds.First().ContestantEntries.Count);
                    Assert.AreEqual("Hans", tourney.Rounds.First().ContestantEntries.First().Contestant.Name);
                }
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void IT_TournementWithJudge()
        {
            long? id = null;
            string judgeName = Guid.NewGuid().ToString();
            try
            {
                // Setup part 1
                using (var db = new NordicArenaDataContext())
                {
                    var judge = new Judge(judgeName);
                    var tourney = new Tournament("I-Test tournament");
                    tourney.Judges = new List<Judge>();
                    tourney.Judges.Add(judge);
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();
                    id = tourney.Id;              
                }
                // Asssert
                using (var db = new EfTournamentService())
                {
                    var tourney = db.GetTournament(id.Value);
                    var judge = tourney.Judges.FirstOrDefault();
                    Assert.AreEqual("I-Test tournament", tourney.Name);
                    Assert.AreEqual(judgeName, judge.Name);
                }
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void IT_Tournament_InitializeDefaults_Round1IsSaved()
        {
            long? id = null;
            try {
                var tournament = new Tournament("I-Test tournament");

                using (var db = new NordicArenaDataContext()) {
                    db.Tournaments.Add(tournament);
                    tournament.InitializeDefaults();
                    db.SaveChanges();
                    id = tournament.Id;
                }
                using (var db = new EfTournamentService())
                {
                    var tourney = db.GetTournament(id.Value);
                    Assert.AreEqual(1, tourney.Rounds.Count(), "RoundCount");
                    Assert.AreEqual(1, tourney.Rounds.FirstOrDefault().RoundNo, "RoundNo");
                }
            }
            finally {
                DatabaseHelper.DeleteTournament(id);
            }
        }
    }
}
