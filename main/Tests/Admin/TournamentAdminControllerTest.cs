using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Admin.Controllers;
using NordicArenaTournament.Areas.Admin.ViewModels;
using NordicArenaTournament.SignalR;
using Tests.Helpers;

namespace Tests.Admin
{
    [TestClass]
    public class TournamentAdminControllerTest
    {
        [TestMethod]
        public void DatabaseFaking_AddAndReadWorks()
        {
            var db = DatabaseFaker.GetFake().Object;
            db.Tournaments.Add(new Tournament());
            var tourneys = db.Tournaments.ToList();
            Assert.AreEqual(1, tourneys.Count);
        }


        [TestMethod]
        public void EditTournamentPost_AddDeleteAndReplaceOk()
        {
            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            var oldTournament = new Tournament();
            oldTournament.Rounds = new HashSet<Round>();
            oldTournament.Rounds.Add(new Round() { Id = 1 });
            oldTournament.Rounds.Add(new Round() { Id = 2 });
            db.AddTournament(oldTournament);

            var newTournament = new Tournament();
            newTournament.Rounds = new HashSet<Round>();
            var newTournamentModel = new EditTournamentViewModel(newTournament);
            newTournamentModel.RoundList.Add(new Round() { Id = 1, Title = "Hi" });
            newTournamentModel.RoundList.Add(new Round() { Id = 3 });

            var target = new TournamentAdminController(db, ServiceFaker.GetFakeFormFeedbackHandler(), new Mock<NaHub>().Object);
            target.SaveTournament(newTournamentModel);

            Assert.IsTrue(oldTournament.Rounds.Any(p => p.Id == 1), "1");
            Assert.IsFalse(oldTournament.Rounds.Any(p => p.Id == 2), "2");
            Assert.IsTrue(oldTournament.Rounds.Any(p => p.Id == 3), "3");
            Assert.AreEqual("Hi", oldTournament.Rounds.FirstOrDefault(p => p.Id == 1).Title);
        }

        [TestMethod]
        public void SaveTournament_StartedTournament_DoesntOverwriteStatus()
        {
            var updatedTournament = new Tournament();
            updatedTournament.InitializeDefaults();
            updatedTournament.Status = TournamentStatus.Prestart;
            var savedTournament = new Tournament();
            savedTournament.InitializeDefaults();
            savedTournament.Status = TournamentStatus.Running;

            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            dbMock.Setup(p => p.GetTournament(It.IsAny<long>())).Returns(savedTournament);

            var target = new TournamentAdminController(db, ServiceFaker.GetFakeFormFeedbackHandler(), new Mock<NaHub>().Object);
            target.SaveTournament(new EditTournamentViewModel(updatedTournament));

            Assert.AreEqual(TournamentStatus.Running, savedTournament.Status);
        }

        [TestMethod]
        public void SaveTournament_StartedTournament_SavesNewName()
        {

            var updatedTournament = new Tournament();
            updatedTournament.InitializeDefaults();
            updatedTournament.Name = "After";

            var savedTournament = new Tournament();
            savedTournament.InitializeDefaults();
            savedTournament.Name = "Before";

            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            dbMock.Setup(p => p.GetTournament(It.IsAny<long>())).Returns(savedTournament);

            var target = new TournamentAdminController(db, ServiceFaker.GetFakeFormFeedbackHandler(), new Mock<NaHub>().Object);
            var updatedTouranmentModel = new EditTournamentViewModel(updatedTournament);
            target.SaveTournament(updatedTouranmentModel);

            Assert.AreEqual("After", savedTournament.Name);
        }


        [TestMethod]
        public void SaveTournament_DeletesRound_ReordersIndicies()
        {

            var updatedTournament = new Tournament();
            updatedTournament.InitializeDefaults();

            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            updatedTournament.Rounds.First().RoundNo = 2;
            int firstRoundNo = 2;
            dbMock.Setup(p => p.GetTournament(It.IsAny<long>())).Returns(updatedTournament);
            dbMock.Setup(p => p.UpdateTournamentAndRounds(It.IsAny<Tournament>())).Callback<Tournament>(
                (tournament) =>
                {
                    firstRoundNo = tournament.Rounds.First().RoundNo;
                });

            var target = new TournamentAdminController(db, null, new Mock<NaHub>().Object);
            var updatedTouranmentModel = new EditTournamentViewModel(updatedTournament);
            target.SaveTournament(updatedTouranmentModel);

            Assert.AreEqual(1, firstRoundNo);
        }

        [TestMethod]
        public void StartTournament_SavesAndStarts()
        {
            var dbMock = DatabaseFaker.GetFake();
            var db = dbMock.Object;
            var controller = new TournamentAdminController(db, ServiceFaker.GetFakeFormFeedbackHandler(), new Mock<NaHub>().Object);
            var tournament = Factory.CreateInitializedTourney()
                .WithJudges(1)
                .WithContestants(4);
            db.AddTournament(tournament);

            var model = new EditTournamentViewModel(tournament);
            model.RoundList[0].ContestantsPerHeat = 3; 
            controller.StartTournament(model);

            Assert.AreEqual(3, tournament.GetRoundNo(1).ContestantsPerHeat);
            Assert.AreEqual(TournamentStatus.Running, tournament.Status);
        }
    }
}
