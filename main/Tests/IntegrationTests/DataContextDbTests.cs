using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaServices;
using NordicArenaTournament.Areas.Judge.Controllers;
using NordicArenaTournament.Areas.Judge.ViewModels;
using NordicArenaTournament.Database;
using NordicArenaTournament.ErrorHandling;
using Tests.Helpers;

namespace Tests.IntegrationTests
{
    [TestClass]
    public class DataContextDbTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void IT_PostRunJudgingsFromWeb_FinalizingJudgementOfOneContestant()
        {

            long? id = null;
            try
            {
                var scores = new ContestantRunViewModel();
                var wrappedScores = new JudgeViewModel();
                // Setup/Act
                using (var db = new NordicArenaDataContext())
                {
                    var tourney = Factory.CreateInitializedTourney();
                    tourney.Contestants.Add(new Contestant("Roger"));
                    tourney.Contestants.Add(new Contestant("Hans"));
                    var judge = new Judge("Dommer");
                    judge.Tournament = tourney;
                    db.Judges.Add(judge);
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();

                    tourney = db.Tournaments.FirstOrDefault(p => p.Id == tourney.Id);
                    tourney.Start();
                    db.SaveChanges();
                    id = tourney.Id;

                    var round1 = tourney.Rounds.FirstOrDefault();
                    round1.RunsPerContestant = 1;
                    var rc1 = round1.ContestantEntries.FirstOrDefault();
                    var critList = tourney.JudgingCriteria.ToList(); 
                    // Set up scores
                    scores.RoundContestantId = rc1.Id;
                    scores.TournamentId = tourney.Id;
                    for (int i = 0; i < critList.Count; i++)
                    {
                        var score = new RunJudging();
                        score.CriterionId = critList[i].Id;
                        score.JudgeId = judge.Id;
                        score.RoundContestantId = rc1.Id;
                        score.RunNo = 1;
                        score.Score = (i + 1);
                        scores.Scores.Add(score);
                    } 
                    
                    wrappedScores.Contestants = new List<ContestantRunViewModel>();
                    wrappedScores.Contestants.Add(scores);
                    wrappedScores.Tournament = tourney;
                }
                // Act
                var ctrl = new TournamentJudgeController(null, new EfTournamentService(), ServiceFaker.GetFakeSignalRHub());
               
                ctrl.JudgeIndex(wrappedScores);
                // Assert
                using (var db = new EfTournamentService())
                {
                    var rc = db.GetRoundContestantGuarded(scores.RoundContestantId);
                    Assert.IsTrue(rc.TotalScore.HasValue);
                }
            }
            catch (DbEntityValidationException exc)
            {
                throw DbValidationExceptionWrapper.Wrap(exc);
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
                ServiceFaker.ResetIocContainer();
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void IT_UpdateRoundContestantDirectly_NoError()
        {

            long? id = null;
            try
            {
                long rcId = 0;
                // Setup/Act
                using (var db = new NordicArenaDataContext())
                {
                    var tourney = Factory.CreateInitializedTourney();
                    tourney.Contestants.Add(new Contestant("Roger"));
                    tourney.Contestants.Add(new Contestant("Hans"));
                    var judge = new Judge("Dommer");
                    judge.Tournament = tourney;
                    db.Judges.Add(judge);
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();

                    tourney = db.Tournaments.FirstOrDefault(p => p.Id == tourney.Id);
                    tourney.Start();
                    db.SaveChanges();
                    id = tourney.Id;
                    var rc1 = tourney.Rounds.FirstOrDefault().ContestantEntries.FirstOrDefault();
                    rcId = rc1.Id;
                }
                using (var db = new EfTournamentService())
                {
                    // Act
                    var rc = db.GetRoundContestantGuarded(rcId);
                    var dummy = rc.Contestant;
                    var dummy2 = rc.Round;// Trigger lazy loading or else ... TODO: Find a better solution. Also used in production code (CalculateTotalScore() in RoundContestant.cs)
                    rc.TotalScore = 1M;
                    db.SaveChanges();
                    // Assert
                    Assert.AreEqual(1M, rc.TotalScore.Value);
                }
            }
            catch (DbEntityValidationException exc)
            {
                throw DbValidationExceptionWrapper.Wrap(exc);
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }
        
        // This one fails with "DbUpdateExcetpion" "while saving entities that do not expose foreign key props". 
        //  can maybe be fixed by exposing foreign Key-IDs in RoundContestant
        //[TestMethod] 
        [TestCategory("Integration")]
        public void StartingTournamentBeforeFirstSave()
        {

            long? id = null;
            try
            {
                // Setup/Act
                using (var db = new NordicArenaDataContext())
                {
                    var tourney = Factory.CreateInitializedTourney();
                    tourney.Contestants.Add(new Contestant("Roger"));
                    tourney.Contestants.Add(new Contestant("Hans"));
                    tourney.Start();
                    db.Tournaments.Add(tourney);
                    db.SaveChanges();
                    id = tourney.Id;
                }
            }
            finally
            {
                DatabaseHelper.DeleteTournament(id);
            }
        }
    }
}
