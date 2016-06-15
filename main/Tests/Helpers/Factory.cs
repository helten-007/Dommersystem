using System.Collections.Generic;
using System.Linq;
using Moq;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Database;

namespace Tests.Helpers
{
    internal static class Factory
    {
        private static int _contId = 1;
        private static int _rcId = 1;

        /// <summary>
        /// Creates a tournament with 1 round. Can be used with integration tests
        /// </summary>
        /// <returns></returns>
        internal static Tournament CreateInitializedTourney()
        {
            var tourney = new Tournament("Tournament");
            tourney.InitializeDefaults();
            return tourney;
        }

        /// <summary>
        /// Creates a started tournament with x number of players. 
        /// Not suitable for integration tests.
        /// </summary>
        internal static Tournament CreateStartedTournament(int playerCount, int runsPerContestants = 1, int contestantsPerHeat = 2)
        {
            var tourney = new Tournament("Tournament");
            tourney.InitializeDefaults();
            tourney.WithContestants(playerCount);
            tourney.Rounds.FirstOrDefault().RunsPerContestant = runsPerContestants;
            tourney.Rounds.FirstOrDefault().ContestantsPerHeat = contestantsPerHeat;
            tourney.Start(new Mock<IListSorter>().Object);
            int rcId = 1;
            foreach (var rc in tourney.Rounds.First().ContestantEntries)
            {
                rc.Id = rcId++;
                rc.EnsureListsAreInitialized();
            }
            return tourney;
        }


        /// <summary>
        /// Creates two rounds: 1 with roundNo=1, 2 with roundno=2 and QualifiesFrom=1
        /// </summary>
        internal static Tournament WithTwoRounds(this Tournament tourney)
        {
            tourney.Rounds.Clear();
            var r1 = new Round()
            {
                Id = 3,
                RoundNo = 1,
                Status = TournamentStatus.Prestart,
                Title = "Innledende"
            };
            var r2 = new Round()
            {
                Id = 4,
                RoundNo = 2,
                Status = TournamentStatus.Prestart,
                Title = "Finale",
                QualifiesFromRound = r1
            };
            r1.EnsureListsAreInitialized();
            r2.EnsureListsAreInitialized();
            tourney.Rounds.Add(r1);
            tourney.Rounds.Add(r2);
            return tourney;
        }

        public static Round WithContestants(this Round round, int count, bool withTotalScore)
        {
            round.ContestantEntries = new HashSet<RoundContestant>();
            for (int i = 0; i < count; i++)
            {
                var contestant = new Contestant("C" + i);
                var rc = new RoundContestant  {
                    Contestant = contestant,
                    Round = round,
                    TotalScore = withTotalScore ? (decimal?) i : null
                };
                round.ContestantEntries.Add(rc);
                rc.EnsureListsAreInitialized();
            }
            return round;
        }

        internal static Contestant WithId(this Contestant c)
        {
            c.Id = _contId++;
            return c;
        }


        internal static Tournament WithJudgingCriteria(this Tournament t, int criteriaCount)
        {
            t.JudgingCriteria.Clear();
            for (int i = 1; i <= criteriaCount; i++)
            {
                t.JudgingCriteria.Add(new JudgingCriterion { Id = i, Title = "Criteria" + i});
            }
            return t;
        }

        internal static Tournament WithJudges(this Tournament t, int judgeCount)
        {
            t.Judges.Clear();
            for (int i = 1; i <= judgeCount; i++)
            {
                var j = new Judge() { Id = i, Name = "J" + i};
                j.EnsureListsAreInitialized();
                t.Judges.Add(j);
            }
            return t;
        }

        /// <summary>
        /// Adds RunJudings with linearly increasing score for all contestants, all runs, all criterias, all judges, in specified round
        /// </summary>
        /// <param name="tourney"></param>
        /// <param name="roundNo">Round number to add for</param>
        /// <returns></returns>
        internal static Tournament WithJudgementsForRound(this Tournament tourney, int roundNo)
        {
            int score = 1;
            var round = tourney.GetRoundNo(roundNo);
            var judgementCount = tourney.GetExpectedJudgementCountPerRun();
            foreach (var rc in round.ContestantEntries)
            {
                foreach (var judge in tourney.Judges)
                {
                    for (int runNo = 1; runNo <= round.RunsPerContestant; runNo++)
                    {
                        foreach (var crit in tourney.JudgingCriteria)
                        {
                            var rj = new RunJudging(rc, judge, crit, runNo, score++);
                            rc.ReplaceRunJudging(rj);
							rc.CalculateTotalScore(judgementCount, round.RunsPerContestant);
                        }
                    }
                }
            }
            return tourney;
        }

        /// <summary>
        /// Initializes tourney with the number of rounds specified
        /// </summary>
        /// <param name="roundCount">roundCount > 1</param>
        internal static Tournament WithRounds(this Tournament tourney, int roundCount)
        {
            // special treatment for round1, since "CreateStartedTournament" might have been called
            var round1 = tourney.GetRoundNo(1);
            if (round1 == null) tourney.Rounds.Add(new Round { RoundNo = 1 });
            for (int i = 2; i <= roundCount; i++)
            {

                var round = new Round() { RoundNo = i, QualifiesFromRound = tourney.GetRoundNo(i-1) };
                tourney.Rounds.Add(round);
            }
            return tourney;
        }

        internal static Tournament WithContestants(this Tournament tourney, int contestantCount)
        {
            for (int i = 0; i < contestantCount; i++)
            {
                var contestant = new Contestant("C" + (i + 1)).WithId();
                contestant.Tournament = tourney;
                tourney.Contestants.Add(contestant);
            }
            return tourney;
        }

        internal static NordicArenaDataContext FeedWith(this NordicArenaDataContext db, Tournament tournament)
        {
            db.Tournaments.Add(tournament);
            foreach (var round in tournament.Rounds)
            {
                db.Rounds.Add(round);
            }
            foreach (var rc in tournament.Rounds.SelectMany(p => p.ContestantEntries).ToList())
            {
                db.RoundContestants.Add(rc);
            }
            foreach (var judge in tournament.Judges)
            {
                db.Judges.Add(judge);
            }
            foreach (var contestant in db.RoundContestants.Select(p => p.Contestant))
            {
                db.Contestants.Add(contestant);
            }
            foreach (var crit in tournament.JudgingCriteria)
            {
                db.Criteria.Add(crit);
            }
            foreach (var rj in db.RoundContestants.SelectMany(p => p.RunJudgings))
            {
                db.RunJudgings.Add(rj);
            }
            return db;
        }

        internal static RunJudging SetIdFromObjects(this RunJudging rj)
        {
            rj.JudgeId = rj.Judge.Id;
            rj.RoundContestantId = rj.RoundContestant.Id;
            rj.CriterionId = rj.Criterion.Id;
            return rj;
        }
    }
}
