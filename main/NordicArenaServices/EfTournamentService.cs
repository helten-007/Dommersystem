using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Database;

namespace NordicArenaServices
{
    public class EfTournamentService : NordicArenaDataContext, ITournamentService
    {
        public virtual Tournament GetTournament(long tournamentId)
        {
            return this.Tournaments.FirstOrDefault(p => p.Id == tournamentId);
        }

        /// <summary>
        /// Returns a list of all tournaments
        /// </summary>
        /// <returns></returns>
        public virtual List<Tournament> GetTournaments()
        {
            return Tournaments.ToList();
        }

        public virtual Tournament GetTournamentGuarded(long tournamentId)
        {
            Tournament tourney = GetTournament(tournamentId);
            if (tourney == null) throw new ArgumentException(String.Format("Could not find Tournament with ID {0}", tournamentId));
            return tourney;
        }

        public virtual void AddTournament(Tournament tournament)
        {
            Tournaments.Add(tournament);
            SaveChanges();
        }

        public virtual RoundContestant GetRoundContestantGuarded(long roundContestantId)
        {
            RoundContestant contestant = RoundContestants.FirstOrDefault(p => p.Id == roundContestantId);
            if (contestant == null) throw new ArgumentException(String.Format("Could not find RoundContestant with ID {0}", roundContestantId));
            return contestant;
        }

        public Judge GetJudgeGuarded(long id)
        {
            var judge = Judges.FirstOrDefault(p => p.Id == id);
            if (judge == null) throw new ArgumentException(String.Format("Could not find judge with ID {0}", id));
            return judge;
        }

        public Contestant GetContestantGuarded(long contestantId)
        {
            var contestant = Contestants.FirstOrDefault(p => p.Id == contestantId);
            if (contestant == null) throw new ArgumentException(String.Format("Could not find contestant with ID {0}", contestantId));
            return contestant;
        }

        public void DeleteTournament(long tournamentId)
        {
            var tourney = GetTournamentGuarded(tournamentId);
            Tournaments.Remove(tourney);
            SaveChanges();
        }

        public void DeleteJudge(long id)
        {
            var entity = GetJudgeGuarded(id);
            Judges.Remove(entity);
            SaveChanges();
        }

        public void DeleteContestant(long contestantId)
        {
            var contestant = GetContestantGuarded(contestantId);
            Contestants.Remove(contestant);
            SaveChanges();
        }

        public void DeleteRounds(Tournament tourney, IList<Round> roundsToRemove)
        {
            foreach (var round in roundsToRemove)
            {
                round.Tournament = null;
                tourney.Rounds.Remove(round);
                Rounds.Remove(round);
            }
            SaveChanges();
        }

        /// <summary>
        /// Adds or replaces juding for particular run
        /// </summary>
        /// <param name="judging"></param>
        public void ReplaceRunJudging(RunJudging judging)
        {
            RunJudgings.Replace(judging);
            SaveChanges();
        }

        /// <summary>
        /// Replaces list of runJudging scores for a single round consteants. Calculates total on RoundContestant if all scores registerd.
        /// </summary>
        public virtual void ReplaceRunJudgings(long tournamentId, long roundContestantId, List<RunJudging> runJudgings)
        {
            Tournament tourney = GetTournamentGuarded(tournamentId);
            RoundContestant contestant = GetRoundContestantGuarded(roundContestantId);
            foreach (var score in runJudgings)
            {
                RunJudgings.Replace(score);
            }
            var expectedJudgeEntriesPerRun = tourney.GetExpectedJudgementCountPerRun();
            contestant.CalculateTotalScore(expectedJudgeEntriesPerRun, contestant.Round.RoundNo, contestant.Round.RunsPerContestant);
            SaveChanges();
        }

        /// <summary>
        /// Replaces ALL runJudgings for all contestants represented in the list passed in. Recalculates totals.
        /// This means that if only judgements for 3 of 5 criteria are sent in for a given contestant,
        /// all 5 existing will still be deleted.
        /// </summary>
        public virtual void ReplaceAllRunJudgings(List<RunJudging> list, Tournament tourney)
        {
            var expectedJudgeEntriesPerRun = tourney.GetExpectedJudgementCountPerRun();
            // Delete the ones we're replacing
            var rcIds = list.Select(p => p.RoundContestantId).Distinct().ToList();
            var oldJudgements = RunJudgings.Where(p => rcIds.Contains(p.RoundContestantId)).ToList();
            foreach (var old in oldJudgements)
            {
                RunJudgings.Remove(old);
            }

            // Insert new ones
            foreach (var score in list)
            {
                RunJudgings.Add(score);
            }

            // Update total score for all round contestants
            foreach (long rcId in rcIds)
            {
                RoundContestant contestant = GetRoundContestantGuarded(rcId);
				contestant.CalculateTotalScore(expectedJudgeEntriesPerRun, contestant.Round.RoundNo, contestant.Round.RunsPerContestant);
            }
            SaveChanges();
        }

        /// <summary>
        /// Adds or updates the contestant entity. If ID == 0, then add, else update. Tournament reference must be set
        /// </summary>
        /// <param name="contestant"></param>
        public void AddOrUpdateContestant(Contestant contestant)
        {
            if (contestant.Tournament == null) throw new ArgumentException("Tournament must be set");
            if (contestant.Id == 0)
            {
                Contestants.Add(contestant);
                if (contestant.Tournament.Status == TournamentStatus.Running)
                {
                    // Add player to heat
                    var round1 = contestant.Tournament.GetRoundNoGuarded(1);
                    var rc = round1.AddContesant(contestant);
                    round1.AssignHeatFor(rc);
                }
            }
            else
            {
                Contestants.Attach(contestant);
                Entry<Contestant>(contestant).State = EntityState.Modified;
                Entry<Tournament>(contestant.Tournament).State = EntityState.Unchanged;
            }
            SaveChanges();
        }

        public void AddJudge(Judge judge)
        {
            if (judge == null) throw new ArgumentException("Judge must be set");
            if (judge.TournamentId == 0) throw new ArgumentException("Tournament ID must be set");
            Judges.Add(judge);
            SaveChanges();
        }

        public void StartTournament(Tournament tournament)
        {
            tournament.Start();
            SaveChanges();
        }

        public virtual void UpdateTournamentAndRounds(Tournament updatedTournament)
        {
            var dbTournament = GetTournamentGuarded(updatedTournament.Id);
            var origStatus = dbTournament.Status;
            var updatedRoundList = updatedTournament.Rounds.ToList();
            dbTournament.Name = updatedTournament.Name;
            dbTournament.ShufflePlayerList = updatedTournament.ShufflePlayerList;
            dbTournament.Status = origStatus; // Status can only be changed through StartTournament
            if (dbTournament.Status == TournamentStatus.Prestart)
            {
                // update rounds
                ReplaceRoundListInto(dbTournament, updatedRoundList);
                dbTournament.AssignRoundQualifyPointers();
                var roundsToDelete = dbTournament.GetRoundsNotInList(updatedRoundList);
                DeleteRounds(dbTournament, roundsToDelete);
            }
            SaveChanges();
        }

        public void NextRun(Tournament tourney)
        {
            tourney.NextRun();
            SaveChanges();
        }

        public void PreviousRun(Tournament tourney)
        {
            tourney.PreviousRun();
            SaveChanges();
        }

        public void EndCurrentRound(Tournament tourney)
        {
            tourney.EndCurrentRound();
            SaveChanges();
        }

        public void SetCurrentRunDone(Tournament tourney)
        {
            tourney.IsCurrentRunDone = true;
            SaveChanges();
        }

        public void ResetRound(long tournamentId, int roundNo)
        {
            var tournament = GetTournamentGuarded(tournamentId);
            var deletedObjects = tournament.ResetRound(roundNo);
            // Workaround for Entity Framework not able to detect deletes through the use of List.Clear();
            foreach (var o in deletedObjects) Entry(o).State = EntityState.Deleted;
            SaveChanges();
        }

        /// <summary>
        /// Insert, updates and deletes rounds in tourney based on list of rounds provided. 
        /// </summary>
        /// <param name="tourney">Tournament object with old list of rounds</param>
        /// <param name="rounds">New list of rounds</param>
        private void ReplaceRoundListInto(Tournament tourney, IEnumerable<Round> rounds)
        {
            if (rounds == tourney.Rounds) throw new ArgumentException("The round list must not be the same as the one in tourney.Rounds");
            foreach (var sourceRound in rounds)
            {
                Round targetRound = null;
                if (sourceRound.Id != 0) targetRound = tourney.Rounds.FirstOrDefault(p => p.Id == sourceRound.Id);
                if (targetRound != null) sourceRound.CopyPropertiesTo(targetRound);
                else
                {
                    tourney.Rounds.Add(sourceRound);
                    targetRound = sourceRound;
                }
                targetRound.Tournament = tourney;
            }
        }

        /// <summary>
        /// Only to be called from DataContext
        /// </summary>
        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}