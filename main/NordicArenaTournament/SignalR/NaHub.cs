using System;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using NordicArenaServices;

namespace NordicArenaTournament.SignalR
{
    /// <summary>
    /// Signal-R-hub for the NordicArena tournament application
    /// </summary>
    public class NaHub : Hub
    {
        private IHubContext _context;
        private ITournamentService _tournamentService;
        public NaHub()
        {
            _tournamentService = DependencyResolver.Current.GetService<ITournamentService>();
        }

        public NaHub(IHubContext context, ITournamentService tournamentService) : this()
        {
            _context = context;
            _tournamentService = tournamentService;
        }

        public virtual void CurrentContestantChanged(long tournamentId)
        {
            if (_context == null) throw new ArgumentException("This method must be called from the server");
            _context.Clients.All.currentContestantChanged();
        }

        public virtual void JudgeScoresSubmitted(long tournamentId)
        {
            if (_context == null) throw new ArgumentException("This method must be called from the server");
            _context.Clients.All.judgeStatusUpdated(tournamentId);
        }

		public virtual void HeadJudgeScoreSubmitted(long tournamentId)
		{
			if (_context == null) throw new ArgumentException("This method must be called from the server");
			_context.Clients.All.resultsUpdated(tournamentId);
		}

        public virtual void SetCurrentRunDone(long tournamentId)
        {
            var tourney = _tournamentService.GetTournamentGuarded(tournamentId);
            _tournamentService.SetCurrentRunDone(tourney);
            Clients.All.runCompleted(tournamentId);
        }
    }
}