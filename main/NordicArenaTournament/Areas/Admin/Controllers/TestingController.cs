using System.Web.Mvc;
using NordicArenaServices;
using NordicArenaTournament.Controllers;

namespace NordicArenaTournament.Areas.Admin.Controllers
{
    public partial class TestingController : TournamentControllerBase
    {
        public virtual ActionResult ScoreAll(long tournamentId, int roundNo)
        {
            var tourney = TournamentService.GetTournament(tournamentId);
            var round = tourney.GetRoundNo(roundNo);
            var list = TestDataFactory.ScoreAllContestants(tourney, round);
            var c = new ContentResult();
            c.Content = "Done";
            return c;
        }
    }
}
