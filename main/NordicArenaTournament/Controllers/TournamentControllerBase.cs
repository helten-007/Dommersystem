using NordicArenaServices;
using NordicArenaTournament.Common;

namespace NordicArenaTournament.Controllers
{
    public abstract class TournamentControllerBase : LayoutControllerBase
    {
        protected ITournamentService TournamentService { get; set; }

        public TournamentControllerBase(FormFeedbackHandler formFeedbackHandler, ITournamentService tournamentService) : base(formFeedbackHandler)
        {
            TournamentService = tournamentService;
        }

        public TournamentControllerBase()
        {
        }
    }
}