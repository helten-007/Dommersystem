using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Speaker.ViewModels;
using Tests.Helpers;

namespace Tests.ViewModels
{
    [TestClass]
    public class SpeakerViewModelTest
    {
        [TestMethod]
        public void SpeakerViewModelCtor_BasicProperties()
        {
            var tournament = Factory.CreateStartedTournament(6, 1, 3);
            var target = new SpeakerViewModel(tournament);
            Assert.AreEqual(1, target.Round.RoundNo, "roundNo");
            Assert.AreEqual(6, target.Round.ContestantEntries.Count, "entris in first round");
            Assert.AreEqual(1, target.CurrentRunNo,"runNo");
            Assert.AreEqual(1, target.CurrentHeatNo, "heatNo");
        }

        [TestMethod]
        public void SpeakerViewModelCtor_Heats()
        {
            var tournament = Factory.CreateStartedTournament(6, 1, 3);
            var target = new SpeakerViewModel(tournament);
            Assert.AreEqual(2, target.Heats.Count);
            Assert.AreEqual(3, target.Heats[0].Contestants.Count, "contestants in heat 1");
            Assert.AreEqual(3, target.Heats[1].Contestants.Count, "contestants in heat 2");
           // Assert.AreSame(target.CurrentContestant.Contestant, target.Heats[0].Contestants[0], "Current is first in heat 1");
        }


        [TestMethod]
        public void SpeakerViewModelCtor_SortedByOrdinal()
        {
            var tournament = Factory.CreateStartedTournament(6, 1, 3);
            var round = tournament.GetRoundsOrdered().ToList()[0];
            var entry1 = round.ContestantEntries.FirstOrDefault(p => p.Contestant.Name == "C1");
            var entry2 = round.ContestantEntries.FirstOrDefault(p => p.Contestant.Name == "C2");
            entry1.Ordinal = 2;
            entry2.Ordinal = 1;
            var target = new SpeakerViewModel(tournament);
            var upperMostContestant = target.Heats[0].Contestants[0];
            Assert.AreEqual("C2", upperMostContestant.Name);

            ServiceFaker.ResetIocContainer();
        }

        [TestMethod]
        public void SpeakerViewModelCtor_PrestartRound_NoExceptions()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Status = TournamentStatus.Running;
            tournament.GetRoundNo(1).Status = TournamentStatus.Prestart;
            tournament.GetRoundNo(1).EnsureListsAreInitialized();

            var target = new SpeakerViewModel(tournament);
            Assert.AreEqual(0, target.Heats.Count);
        }

        [TestMethod]
        public void RunControls_EnableNextRun_AtFirstOfTwoRuns_True()
        {
            // Må være lov selv om ikke alle dommerkarakterer har kommet inn for å tilfredsstille krav om at 
            // konkurransen kan gjennomføres med annen deltakerrekkefølge enn den som er satt opp av systemet
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            var round = tournament.Rounds.FirstOrDefault();

            var result = new SpeakerViewModel.RunControls(tournament, round);
            
            Assert.IsTrue(result.EnableNextRun);
        }

        [TestMethod]
        public void RunControls_EnableNextRun_AtSecondOfTwoRunsLastHeat_False()
        {
            var tournament = Factory.CreateStartedTournament(4, 2, 2);
            var round = tournament.Rounds.FirstOrDefault();
            tournament.GetRoundCounter().SetValue(2, 2); // Heat 2 run 2

            var result = new SpeakerViewModel.RunControls(tournament, round);

            Assert.IsFalse(result.EnableNextRun);
        }

        [TestMethod]
        public void RunControls_EnableNextRun_SecondRunFirstHeat_True()
        {
            var tournament = Factory.CreateStartedTournament(4, 2, 2);
            var round = tournament.Rounds.FirstOrDefault();
            tournament.GetRoundCounter().SetValue(1, 2);

            var result = new SpeakerViewModel.RunControls(tournament, round);

            Assert.IsTrue(result.EnableNextRun);
        }

        [TestMethod]
        public void RunControls_EnableNextRun_SecondRunFirstHeat1PerHeat_True()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            var round = tournament.Rounds.FirstOrDefault();
            tournament.GetRoundCounter().SetValue(1, 2);

            var result = new SpeakerViewModel.RunControls(tournament, round);

            Assert.IsTrue(result.EnableNextRun);
        }

        [TestMethod]
        public void RunControls_EnablePreviousRun_AtSecondRun_True()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            var round = tournament.Rounds.FirstOrDefault();
            tournament.GetRoundCounter().SetValue(1,2);

            var result = new SpeakerViewModel.RunControls(tournament, round);

            Assert.IsTrue(result.EnablePreviousRun);
        }

        [TestMethod]
        public void RunControls_EnablePreviousRun_AtSecondContestantButRoundEnded_False()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            var round = tournament.Rounds.FirstOrDefault();
            tournament.GetRoundCounter().SetValue(2, 1);
            round.Status = TournamentStatus.Ended;

            var result = new SpeakerViewModel.RunControls(tournament, round);

            Assert.IsFalse(result.EnablePreviousRun);
        }

        [TestMethod]
        public void SpeakerViewModelCtor_Running_StartButtonEnabled()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            tournament.Rounds.FirstOrDefault().Status = TournamentStatus.Running;

            var result = new SpeakerViewModel(tournament, 1);

            Assert.AreEqual(false, result.DisableStartButton);
        }

        [TestMethod]
        public void SpeakerViewModelCtor_Ended_StartButtonDisabled()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            tournament.Rounds.FirstOrDefault().Status = TournamentStatus.Ended;

            var result = new SpeakerViewModel(tournament, 1);

            Assert.AreEqual(true, result.DisableStartButton);
        }

        [TestMethod]
        public void SpeakerViewModelCtor_PreStart_StartButtonDisabled()
        {
            var tournament = Factory.CreateStartedTournament(2, 2, 1);
            tournament.Rounds.FirstOrDefault().Status = TournamentStatus.Prestart;

            var result = new SpeakerViewModel(tournament, 1);

            Assert.AreEqual(true, result.DisableStartButton);
        }

        [TestMethod]
        public void SpeakerViewModelCtor_4Contestants2Heats_2ContestantsReturned()
        {
            var tourney = Factory.CreateStartedTournament(4, 1, 2);
            
            var result = new SpeakerViewModel(tourney, 1);

            Assert.AreEqual(2, result.CurrentContestants.Count);
        }
    }
}

