using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NordicArenaDomainModels.Models;
using NordicArenaDomainModels.ObjectValidation;
using NordicArenaServices;
using NordicArenaTournament.Areas.Admin.ViewModels;
using NordicArenaTournament.Common;
using NordicArenaTournament.Controllers;
using NordicArenaTournament.SignalR;

namespace NordicArenaTournament.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for admin-pages in the context of a tournament
    /// </summary>
    public partial class TournamentAdminController : TournamentControllerBase
    {
        private readonly NaHub _hub;
        private const string IdToHighlightKey = "IdToHighlight";

        public TournamentAdminController(ITournamentService tournamentService, FormFeedbackHandler formFeedbackHandler, NaHub naHub)
            : base(formFeedbackHandler, tournamentService)
        {
            _hub = naHub;
        }

        public virtual ActionResult AdminIndex(long tournamentId)
        {
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.EditTournament(tournamentId));
        }

        public virtual ActionResult ContestantList(long tournamentId, long? contestantId = null)
        {
            var tournament = TournamentService.GetTournamentGuarded(tournamentId);
            var mode = contestantId == null ? EntityMode.Create : EntityMode.Edit;
            var model = new ContestantListAndCreate(tournament, mode);
            if (contestantId != null) model.Contestant = TournamentService.GetContestantGuarded(contestantId.Value);
            model.IdToHighlight = (long?)Session[IdToHighlightKey];
            Session[IdToHighlightKey] = null; //Reset for consecutive refreshes
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult ContestantList(long tournamentId, ContestantListAndCreate model)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            model.Contestant.Tournament = tourney;
            TournamentService.AddOrUpdateContestant(model.Contestant);
            Session[IdToHighlightKey] = model.Contestant.Id;
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.ContestantList(tournamentId, new long?()));
        }

        public virtual ActionResult DeleteContestant(long tournamentId, long? id)
        {
            if (id == null) throw new ArgumentException("Contestant ID must be set");
            TournamentService.DeleteContestant(id.Value);
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.ContestantList(tournamentId, new long?()));
        }

        public virtual ActionResult RemoveContestant(long tournamentId, long? id)
        {
            if (id == null) throw new ArgumentException("Contestant ID must be set");
            // Todo: implement logic for removing contestant from running tournament
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.ContestantList(tournamentId, new long?()));
        }

        public virtual ActionResult JudgeList(long tournamentId)
        {
            var tournament = TournamentService.GetTournamentGuarded(tournamentId);
            var model = new JudgeListAndCreate(tournament);
            model.IdToHighlight = (long?) Session["IdToHighlight"];
            Session[IdToHighlightKey] = null;
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult JudgeList(long tournamentId, JudgeListAndCreate model)
        {
            model.NewJudge.GenerateLoginCode();
            model.NewJudge.TournamentId = tournamentId;
            TournamentService.AddJudge(model.NewJudge);
            // By redirecting to GET-method, refresh (F5) works better at client      
            Session[IdToHighlightKey] = model.NewJudge.Id;
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.JudgeList(tournamentId));
        }

        public virtual ActionResult DeleteJudge(long tournamentId, long id)
        {
            TournamentService.DeleteJudge(id);
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.JudgeList(tournamentId));
        }

        public virtual ActionResult EditTournament(long tournamentId)
        {
            var tournament = TournamentService.GetTournamentGuarded(tournamentId);
            var model = new EditTournamentViewModel(tournament);
            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "SaveTournament")]
        public virtual ActionResult SaveTournament(EditTournamentViewModel model)
        {
            var tourney = TournamentService.GetTournamentGuarded(model.Tournament.Id);
            model.IndexToRoundNo();
            model.Tournament.Rounds = model.RoundList;
            TournamentService.UpdateTournamentAndRounds(model.Tournament);
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.EditTournament(tourney.Id));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "StartTournament")]
        public virtual ActionResult StartTournament(EditTournamentViewModel model)
        {
            SaveTournament(model);
            var tourney = TournamentService.GetTournamentGuarded(model.Tournament.Id);
            ValidationResultSet result;                 
            if (!tourney.CanBeStarted(out result))
            {
                FormFeedbackHandler.SetError(HttpContext, result.GetFirstErrorMessage());
            }
            else
            {
                TournamentService.StartTournament(tourney);
            }
            return this.RedirectToAction(MVC.Admin.TournamentAdmin.EditTournament(tourney.Id));
        }

        // Called from javascript
        public virtual ActionResult EditTournamentRound(int id)
        {
            var model = new EditTournamentViewModel(); // Dummy container object
            model.RoundIx = id;
            model.RoundList = new List<Round>(id + 1);
            model.RoundCount = id + 1;
            for (int i = 0; i < id + 1; i++ ) model.RoundList.Add(new Round());
            return PartialView(model);
        }

        public virtual ActionResult Results(long tournamentId, int roundNo = 1)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var model = new ResultsViewModel(tourney, roundNo);
            return View(model);
        }

		public virtual ActionResult ResultsContent(long tournamentId, int roundNo = 1)
		{
			var tourney = TournamentService.GetTournamentGuarded(tournamentId);
			var model = new ResultsViewModel(tourney, roundNo);
			return PartialView(model);
		}

        public virtual ActionResult ResetRound(long tournamentId, int roundNo)
        {
            TournamentService.ResetRound(tournamentId, roundNo);
            _hub.CurrentContestantChanged(tournamentId);
            return RedirectToAction(MVC.Admin.TournamentAdmin.EditTournament(tournamentId));
        }
    }
}

