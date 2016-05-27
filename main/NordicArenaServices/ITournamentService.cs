using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;

namespace NordicArenaServices
{
    /// <summary>
    /// An abstraction layer for the data access. 
    /// </summary>
    public interface ITournamentService : IDisposable
    {
        Tournament GetTournament(long tournamentId);

        /// <summary>
        /// Returns a list of all tournaments
        /// </summary>
        /// <returns></returns>
        List<Tournament> GetTournaments();

        Tournament GetTournamentGuarded(long tournamentId);
        void AddTournament(Tournament tournament);
        RoundContestant GetRoundContestantGuarded(long roundContestantId);
        Judge GetJudgeGuarded(long id);
        Contestant GetContestantGuarded(long contestantId);
        void DeleteTournament(long tournamentId);
        void DeleteJudge(long id);
        void DeleteContestant(long contestantId);
        void DeleteRounds(Tournament tourney, IList<Round> roundsToRemove);

        /// <summary>
        /// Adds or replaces juding for particular run
        /// </summary>
        /// <param name="judging"></param>
        void ReplaceRunJudging(RunJudging judging);

        /// <summary>
        /// Replaces list of runJudging scores as well as calculates total on RoundContestant if done.
        /// Less optimized than ReplaceRunJudgings(ContestantRunViewModel model), as it fetches roundContestant for each runJudging line
        /// </summary>
        void ReplaceAllRunJudgings(List<RunJudging> list, Tournament tourney);

        /// <summary>
        /// Adds or updates the contestant entity. If ID == 0, then add, else update. Tournament reference must be set
        /// </summary>
        /// <param name="contestant"></param>
        void AddOrUpdateContestant(Contestant contestant);

        /// <summary>
        /// Adds a judge to the database
        /// </summary>
        /// <param name="judge"></param>
        void AddJudge(Judge judge);

        /// <summary>
        /// Saves any uncommited changes
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Starts a tournament and initializes first round
        /// </summary>
        /// <param name="tourney"></param>
        void StartTournament(Tournament tournament);

        /// <summary>
        /// Update the tournament data including the list of rounds
        /// </summary>
        /// <param name="updatedTournament">Tournament with updated properties</param>
        void UpdateTournamentAndRounds(Tournament updatedTournament);

        /// <summary>
        /// Advances given tournament to next Run
        /// </summary>
        /// <param name="tourney"></param>
        void NextRun(Tournament tourney);

        /// <summary>
        /// Goes back one Run in the given tournament
        /// </summary>
        /// <param name="tourney"></param>
        void PreviousRun(Tournament tourney);

        /// <summary>
        /// Ends current round in given tournament
        /// </summary>
        /// <param name="tourney"></param>
        void EndCurrentRound(Tournament tourney);

        /// <summary>
        /// Marks the current run as done in given tournament
        /// </summary>
        /// <param name="tourney"></param>
        void SetCurrentRunDone(Tournament tourney);

        /// <summary>
        /// Deletes results and reshuffles contestant list for this round
        /// </summary>
        void ResetRound(long tournamentId, int roundNo);

        void ReplaceRunJudgings(long tournamentId, long roundContestantId, List<RunJudging> runJudgings);
    }
}
