using Moq;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Speaker.ViewModels;
using Tests.Helpers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Models
{
    [TestClass]
    public partial class TournamentTest
    {
        [TestMethod]
        public void AssignRoundQualifyPointers_ThreeRounds_AllOk()
        {
            var target = new Tournament();
            var rounds = new List<Round>();
            rounds.Add(new Round() { Id = 1, RoundNo = 1 });
            rounds.Add(new Round() { Id = 2, RoundNo = 2 });
            rounds.Add(new Round() { Id = 3, RoundNo = 3 });
            target.Rounds = rounds;

            target.AssignRoundQualifyPointers();

            var round1 = target.Rounds.FirstOrDefault(p => p.Id == 1);
            var round2 = target.Rounds.FirstOrDefault(p => p.Id == 2);
            var round3 = target.Rounds.FirstOrDefault(p => p.Id == 3);

            Assert.AreSame(null, round1.QualifiesFromRound);
            Assert.AreSame(round1, round2.QualifiesFromRound);
            Assert.AreSame(round2, round3.QualifiesFromRound);
        }

        [TestMethod]
        public void TournamentCtor_SetsTournamentStatuPrestart()
        {
            var target = new Tournament();
            Assert.AreEqual(TournamentStatus.Prestart, target.Status);
        }

        [TestMethod]
        public void TournamentInitialize_SetsAtLeastOneJudgeCriterion()
        {
            var target = new Tournament();
            target.InitializeDefaults();
            Assert.IsTrue(target.JudgingCriteria.Count > 0);
        }

        [TestMethod]
        public void CanBeStarted_NoContestants_False()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Judges.Add(new Judge("Peter"));
            var result = tournament.CanBeStarted();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeStarted_OneInvalidContestant_DoesntMatter()
        {
            // Not the responsibility of the Tournament object to validate
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant()); // No name = invalid
            tournament.Judges.Add(new Judge("Peter"));
            var result = tournament.CanBeStarted();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeStarted_OneInvalidJudgeCriterion_False()
        {
            // Not the responsibility of the Tournament object to validate
            var tournament = Factory.CreateInitializedTourney();
            var crit = new JudgingCriterion();
            crit.Title = null; // invalid
            tournament.JudgingCriteria.Add(crit);
            var result = tournament.CanBeStarted();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeStarted_OneContestants_True()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("Roger"));
            tournament.Judges.Add(new Judge("Peter"));
            var result = tournament.CanBeStarted();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeStarted_NoJudges_False()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Judges.Clear();
            tournament.Contestants.Add(new Contestant("Roger"));
            var result = tournament.CanBeStarted();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeStarted_AlreadyStarted1_False()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("Ola"));
            tournament.Status = TournamentStatus.Running;
            var result = tournament.CanBeStarted();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeStarted_AlreadyStarted2_False()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("Ola"));
            tournament.Status = TournamentStatus.Ended;
            var result = tournament.CanBeStarted();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Start_4Contestants2PerHeat_DistributionAndHeatNoSet()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));
            tournament.Contestants.Add(new Contestant("c"));
            tournament.Contestants.Add(new Contestant("d"));
            var round1 = tournament.Rounds.FirstOrDefault();
            round1.ContestantsPerHeat = 2;

            tournament.Start();

            Assert.AreEqual(4, round1.ContestantEntries.Count);
            Assert.AreEqual(2, round1.ContestantEntries.Count(p => p.HeatNo == 2), "Heat1");
            Assert.AreEqual(2, round1.ContestantEntries.Count(p => p.HeatNo == 2), "Heat2");
        }


        [TestMethod]
        public void Start_4Contestants4PerHeat_OrdinalsSet()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));
            tournament.Contestants.Add(new Contestant("c"));
            tournament.Contestants.Add(new Contestant("d"));
            var round1 = tournament.Rounds.FirstOrDefault();
            round1.ContestantsPerHeat = 2;

            tournament.Start();
            var entries = round1.ContestantEntries.OrderBy(p => p.Ordinal).ToList();

            for (int i = 0; i < 4; i++) 
            {
                Assert.AreEqual(i + 1, entries[i].Ordinal);
            }
        }

        [TestMethod]
        public void Start_4Contestants8PerHeat_1heat()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));
            tournament.Contestants.Add(new Contestant("c"));
            tournament.Contestants.Add(new Contestant("d"));
            var round1 = tournament.Rounds.FirstOrDefault();
            round1.ContestantsPerHeat = 8;

            tournament.Start();

            Assert.AreEqual(4, round1.ContestantEntries.Count);
            Assert.AreEqual(4, round1.ContestantEntries.Count(p => p.HeatNo == 1), "Heat1");
        }
        
        [TestMethod]
        public void Start_2Contestants0PerHeat_TreatedAsInfinite()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));
            var round1 = tournament.Rounds.FirstOrDefault();
            round1.ContestantsPerHeat = 0;

            tournament.Start();

            Assert.AreEqual(2, round1.ContestantEntries.Count);
            Assert.AreEqual(2, round1.ContestantEntries.Count(p => p.HeatNo == 1), "Heat1");
        }

        [TestMethod]
        public void Start_StatusUpdated()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));

            tournament.Start();

            Assert.AreEqual(TournamentStatus.Running, tournament.Status);
        }

        [TestMethod]
        public void Start_AtRunSet()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));

            tournament.Start();

            Assert.AreEqual(1, tournament.GetCurrentRunNo());
        }

        [TestMethod]
        public void Start_RoundStatusSet()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Contestants.Add(new Contestant("a"));

            tournament.Start();

            Assert.AreEqual(TournamentStatus.Running, tournament.Rounds.FirstOrDefault().Status);
        }

        [TestMethod]
        public void Start_1CPH_1Heat()
        {
            var tournament = Factory.CreateInitializedTourney();
            var round = tournament.Rounds.FirstOrDefault();
            round.ContestantsPerHeat = 1;
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));

            tournament.Start();

            Assert.AreEqual(1, round.ContestantEntries.ToList()[0].HeatNo);
            Assert.AreEqual(2, round.ContestantEntries.ToList()[1].HeatNo);

        }

        [TestMethod]
        public void Start_0CPH_1Heat()
        {
            var tournament = Factory.CreateInitializedTourney();
            var round = tournament.Rounds.FirstOrDefault();
            round.ContestantsPerHeat = 0;
            tournament.Contestants.Add(new Contestant("a"));
            tournament.Contestants.Add(new Contestant("b"));

            tournament.Start();

            Assert.IsTrue(round.ContestantEntries.All(p => p.HeatNo == 1));
        }

        [TestMethod]
        public void InitializeDefaults_FirstRoundStatus_Presstart()
        {
            var tournament = new Tournament();
            tournament.InitializeDefaults();
            Assert.AreEqual(TournamentStatus.Prestart, tournament.Rounds.FirstOrDefault().Status);
        }

        [TestMethod]
        public void GetCurrentRound_AfterLast_Null()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Ended;
            tournament.GetRoundNo(2).Status = TournamentStatus.Ended;
            var result = tournament.GetCurrentRound();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCurrentRound_At2Of2_2()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Ended;
            tournament.GetRoundNo(2).Status = TournamentStatus.Running;
            var result = tournament.GetCurrentRound();
            Assert.AreEqual(2, result.RoundNo);
        }

        [TestMethod]
        public void GetCurrentRound_Between1And2_2()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Ended;
            tournament.GetRoundNo(2).Status = TournamentStatus.Prestart;
            var result = tournament.GetCurrentRound();
            Assert.AreEqual(2, result.RoundNo);
        }

        [TestMethod]
        public void GetCurrentRound_At1of2_1()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Running;
            tournament.GetRoundNo(2).Status = TournamentStatus.Prestart;
            var result = tournament.GetCurrentRound();
            Assert.AreEqual(1, result.RoundNo);
        }
        [TestMethod]
        public void GetCurrentOrDefaultRound_At1_1()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Running;
            tournament.GetRoundNo(2).Status = TournamentStatus.Prestart;
            var result = tournament.GetCurrentOrDefaultRound();
            Assert.AreEqual(1, result.RoundNo);
        }

        [TestMethod]
        public void GetCurrentOrDefaultRound_AfterLast_Last()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundNo(1).Status = TournamentStatus.Ended;
            tournament.GetRoundNo(2).Status = TournamentStatus.Ended;
            var result = tournament.GetCurrentOrDefaultRound();
            Assert.AreEqual(2, result.RoundNo);
        }

        [TestMethod]
        public void PreviousRun_ResetsIsCurrentRunDone()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.GetRoundCounter().SetValue(1, 2);
            tournament.IsCurrentRunDone = true;
            tournament.PreviousRun();

            Assert.AreEqual(false, tournament.IsCurrentRunDone);
        }

        [TestMethod]
        public void NextRun_ResetsIsCurrentRunDone()
        {
            var tournament = Factory.CreateStartedTournament(playerCount: 2, runsPerContestants: 2);
            tournament.IsCurrentRunDone = true;
            tournament.NextRun();

            Assert.AreEqual(false, tournament.IsCurrentRunDone);
        }

        [TestMethod]
        public void GetNextRound_AtFirstRoundByState_SecondRound()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().First().Status = TournamentStatus.Running;

            var result = tournament.GetNextRound();

            Assert.AreEqual(2, result.RoundNo);
        }

        [TestMethod]
        public void GetPreviousRound_1RoundAfter1_1()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;

            var result = tournament.GetPreviousRound();

            Assert.AreSame(tournament.GetRoundsOrdered().ToList()[0], result);
        }

        [TestMethod]
        public void GetPreviousRound_AtNull_Null()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Prestart;

            var result = tournament.GetPreviousRound();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetPreviousRound_2RounsdAt2_R1()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;
            tournament.GetRoundsOrdered().ToList()[1].Status = TournamentStatus.Running;

            var result = tournament.GetPreviousRound();

            Assert.AreSame(tournament.GetRoundsOrdered().ToList()[0], result);
        }
        
        [TestMethod]
        public void GetPreviousRound_2RounsdAfer2_R2()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;
            tournament.GetRoundsOrdered().ToList()[1].Status = TournamentStatus.Ended;

            var result = tournament.GetPreviousRound();

            Assert.AreSame(tournament.GetRoundsOrdered().ToList()[1], result);
        }

        [TestMethod]
        public void GetPreviousRound_2RounsdAt2RoundContestantSet_R1()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;
            tournament.GetRoundsOrdered().ToList()[1].Status = TournamentStatus.Running;

            var result = tournament.GetPreviousRound();

            Assert.AreSame(tournament.GetRoundsOrdered().ToList()[0], result);
        }

        [TestMethod]
        public void HasNextRound_NoRounds_False()
        {
            var tournament = new Tournament();
            tournament.Rounds = new HashSet<Round>();
            var result = tournament.HasNextRound();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasNextRound_1RoundAt1_False()
        {
            var tournament = Factory.CreateStartedTournament(1, 1, 1);
            var result = tournament.HasNextRound();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPreviousRound_1RoundAt0_False()
        {
            var tournament = Factory.CreateInitializedTourney();
            tournament.Rounds.Add(new Round());

            var result = tournament.HasPreviousRound();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPreviousRound_1RoundAt1_False()
        {
            var tournament = Factory.CreateStartedTournament(1,1,1);

            var result = tournament.HasPreviousRound();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPreviousRound_2RounsdAt2_True()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;
            tournament.GetRoundsOrdered().ToList()[1].Status = TournamentStatus.Running;

            var result = tournament.HasPreviousRound();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasPreviousRound_2RounsdAfter2_True()
        {
            var tournament = Factory.CreateInitializedTourney().WithTwoRounds();
            tournament.GetRoundsOrdered().ToList()[0].Status = TournamentStatus.Ended;
            tournament.GetRoundsOrdered().ToList()[1].Status = TournamentStatus.Ended;

            var result = tournament.HasPreviousRound();

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void EndCurrentRound_2of4PlayersAdvance()
        {
            var tournament = Factory.CreateStartedTournament(4, 1, 4);
            var round1 = tournament.GetRoundNo(1);
            var round2 = new Round() { RoundNo = 2, QualifiesFromRound = round1 };
            round2.MaxContestants = 2;
            tournament.Rounds.Add(round2);
            if (tournament.GetRoundNo(1).ContestantEntries.Count != 4) throw new TestSetupException();
            if (tournament.GetRoundNo(2).ContestantEntries != null) throw new TestSetupException();
            // ASsign scores
            var contestantsOrdered = round1.GetContestantEntriesOrdered();
            contestantsOrdered[0].TotalScore = 10;
            contestantsOrdered[1].TotalScore = 4;
            contestantsOrdered[2].TotalScore = 16;
            contestantsOrdered[3].TotalScore = 7;
            // Start round 2
            var result = tournament.EndCurrentRound();

            // Assert
            Assert.IsTrue(result.IsTrue, result.Reason);
            Assert.AreEqual(2, round2.ContestantEntries.Count);
            var flatContestants = round2.ContestantEntries.Select(p => p.Contestant).ToList();
            Assert.IsTrue(flatContestants.Contains(contestantsOrdered[2].Contestant), "Contestant[2] missing");
            Assert.IsTrue(flatContestants.Contains(contestantsOrdered[0].Contestant), "Contestant[0] missing");
            Assert.AreEqual(TournamentStatus.Ended, round1.Status);
            Assert.AreEqual(TournamentStatus.Running, round2.Status);
            Assert.AreEqual(TournamentStatus.Running, tournament.Status);
        }

        [TestMethod]
        public void EndCurrentRound_Round1Of1_TournamentEnded()
        {
            var tournament = Factory.CreateStartedTournament(4, 1, 4);
            var round1 = tournament.GetRoundNo(1);
            if (tournament.GetRoundNo(1).ContestantEntries.Count != 4) throw new TestSetupException();
            // ASsign scores
            var contestantsOrdered = round1.GetContestantEntriesOrdered();
            contestantsOrdered[0].TotalScore = 10;
            contestantsOrdered[1].TotalScore = 4;
            contestantsOrdered[2].TotalScore = 16;
            contestantsOrdered[3].TotalScore = 7;
            // end round and tournament
            var result = tournament.EndCurrentRound();

            // Assert
            Assert.IsTrue(result.IsTrue, result.Reason);
            Assert.AreEqual(TournamentStatus.Ended, round1.Status);
            Assert.AreEqual(TournamentStatus.Ended, tournament.Status);
        }

        [TestMethod]
        public void EndCurrentRound_Round1Of2_RoundCounterReset()
        {
            var tournament = Factory.CreateStartedTournament(8, 2, 2)
                .WithRounds(2)
                .WithJudges(1)
                .WithJudgingCriteria(1)
                .WithJudgementsForRound(1);
            tournament.Rounds.Last().MaxContestants = 2;

            tournament.SetRoundCounter(4,2);

            tournament.EndCurrentRound();
            var counter = tournament.GetRoundCounter();
            counter = tournament.GetRoundCounter();

            Assert.AreEqual(1, counter.GetHeatNo());
            Assert.AreEqual(1, counter.GetRunNo());
        }



        [TestMethod]
        public void ResetCurrentRound_Round1Of2InSecondHeat()
        {
            var tournament = Factory.CreateStartedTournament(8, 2, 2)
                .WithRounds(2)
                .WithJudges(1)
                .WithJudgingCriteria(1)
                .WithJudgementsForRound(1);
            tournament.Rounds.Last().MaxContestants = 2;
            tournament.SetRoundCounter(2, 2);

            tournament.ResetRound(1);

            var counter = tournament.GetRoundCounter();
            var round1 = tournament.GetRoundNo(1);
            Assert.AreEqual(1, counter.GetHeatNo(), "heat");
            Assert.AreEqual(1, counter.GetRunNo(), "run");
            Assert.AreEqual(TournamentStatus.Prestart, round1.Status, "Round status");
            Assert.AreEqual(TournamentStatus.Prestart, tournament.Status, "Tournament status");
            Assert.AreEqual(0, round1.ContestantEntries.Count, "contestantEntry count");
        }

        [TestMethod]
        public void ResetCurrentRound_Round2Of2InSecondHeat()
        {
            var tournament = Factory.CreateStartedTournament(8, 2, 2)
                .WithRounds(2)
                .WithJudges(1)
                .WithJudgingCriteria(1)
                .WithJudgementsForRound(1);
            var round1 = tournament.GetRoundNo(1);
            var round2 = tournament.GetRoundNo(2);
            round2.MaxContestants = 2;
            round1.End();
            round2.Start(new Mock<IListSorter>().Object);
            tournament.SetRoundCounter(2, 2);

            tournament.ResetRound(2);

            var counter = tournament.GetRoundCounter();
            Assert.AreEqual(1, counter.GetHeatNo(), "heat");
            Assert.AreEqual(1, counter.GetRunNo(), "run");
            Assert.AreEqual(TournamentStatus.Running, tournament.Status, "Tournament");
            Assert.AreEqual(TournamentStatus.Running, round1.Status, "Round 1");
            Assert.AreEqual(TournamentStatus.Prestart, round2.Status, "Round 2");
            Assert.AreEqual(8, round1.ContestantEntries.Count, "R1 contestantEntry count");
            Assert.AreEqual(0, round2.ContestantEntries.Count, "R2 contestantEntry count");
        }

        [TestMethod]
        public void ResetCurrentRound_ResetRound1WhenRound2Of2InSecondHeat()
        {
            var tournament = Factory.CreateStartedTournament(8, 2, 2)
                .WithRounds(2)
                .WithJudges(1)
                .WithJudgingCriteria(1)
                .WithJudgementsForRound(1);
            var round1 = tournament.GetRoundNo(1);
            var round2 = tournament.GetRoundNo(2);
            round2.MaxContestants = 2;
            round1.End();
            round2.Start(new Mock<IListSorter>().Object);
            tournament.SetRoundCounter(2, 2);

            tournament.ResetRound(1);

            var counter = tournament.GetRoundCounter();
            Assert.AreEqual(1, counter.GetHeatNo(), "heat");
            Assert.AreEqual(1, counter.GetRunNo(), "run");
            Assert.AreEqual(TournamentStatus.Prestart, tournament.Status, "Tournament");
            Assert.AreEqual(TournamentStatus.Prestart, round1.Status, "Round 1");
            Assert.AreEqual(TournamentStatus.Prestart, round2.Status, "Round 2");
            Assert.AreEqual(0, round1.ContestantEntries.Count, "R1 contestantEntry count");
            Assert.AreEqual(0, round2.ContestantEntries.Count, "R2 contestantEntry count");
        }

        [TestMethod]
        public void GetExpectedJudgementCountPerRun()
        {
            Tournament t = Factory.CreateInitializedTourney();
            var round = t.Rounds.First();
            round.RunsPerContestant = 5;
            t.Judges.Add(new Judge());
            t.Judges.Add(new Judge());
            t.JudgingCriteria.Clear();
            t.JudgingCriteria.Add(new JudgingCriterion());
            t.JudgingCriteria.Add(new JudgingCriterion());
            t.JudgingCriteria.Add(new JudgingCriterion());
            
            var result = t.GetExpectedJudgementCountPerRun();

            Assert.AreEqual(6, result); // 3*2
        }

        [TestMethod]
        public void GetJudgeStatusListFor() 
        {
            var tourney = Factory.CreateInitializedTourney();
        }

        [TestMethod]
        public void GetJudgeStatusListFor_NullRc_NoException()
        {
            var t = new Tournament();
            var res = JudgeHasScoredTuple.GetJudgeStatusListFor(t, null, 1);
            Assert.IsNotNull(res);
            Assert.AreEqual(0, res.Count);
        }

        [TestMethod]
        public void GetJudgeStatusListForCurrentHeat_TwoJudgesOneDone()
        {
            var t = Factory.CreateStartedTournament(2, 1, 1).WithJudgingCriteria(2).WithJudges(2);
            var j1 = t.Judges.ToList()[0];
            var j2 = t.Judges.ToList()[1];
            var round = t.Rounds.FirstOrDefault();
            t.GetRoundCounter().SetValue(2, 1);
            // Judge 1 
            j1.RunJudgings.Add(new RunJudging {CriterionId = 1, JudgeId = 1, RoundContestantId = 1, RunNo = 1});
            j1.RunJudgings.Add(new RunJudging {CriterionId = 2, JudgeId = 1, RoundContestantId = 1, RunNo = 1});
            j1.RunJudgings.Add(new RunJudging {CriterionId = 1, JudgeId = 1, RoundContestantId = 2, RunNo = 1});
            j1.RunJudgings.Add(new RunJudging {CriterionId = 2, JudgeId = 1, RoundContestantId = 2, RunNo = 1});
            // Judge 2
            j1.RunJudgings.Add(new RunJudging {CriterionId = 1, JudgeId = 2, RoundContestantId = 1, RunNo = 1});
            j1.RunJudgings.Add(new RunJudging {CriterionId = 2, JudgeId = 2, RoundContestantId = 1, RunNo = 1});

            // Act
            var res = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(t);

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual(true, res[0].HasJudged);
            Assert.AreEqual(false, res[1].HasJudged);
        }
    }
}
