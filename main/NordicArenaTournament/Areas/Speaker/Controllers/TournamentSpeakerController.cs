using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NordicArenaDomainModels.Models;
using NordicArenaServices;
using NordicArenaTournament.Areas.Speaker.ViewModels;
using NordicArenaTournament.Common;
using NordicArenaTournament.Controllers;
using NordicArenaTournament.SignalR;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Speaker.Controllers
{
    public partial class TournamentSpeakerController : TournamentControllerBase
    {
        private NaHub _hub;

        public TournamentSpeakerController() { }

        public TournamentSpeakerController(FormFeedbackHandler formFeedbackHandler, ITournamentService tournamentService, NaHub naHub)
            : base(formFeedbackHandler, tournamentService)
        {
            _hub = naHub;
        }

        [OutputCacheAttribute(Duration = 0, NoStore = true)]
        public virtual ActionResult SpeakerIndex(long tournamentId, int? roundNo = null)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            if (tourney.Status != TournamentStatus.Running) return this.View(MVC.Speaker.TournamentSpeaker.Views.ViewNames.TournamentNotRunning, new _LayoutViewModel());
            var model = new SpeakerViewModel(tourney, roundNo);
            return View(model);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "NextContestant")]
        public virtual ActionResult NextContestant(long tournamentId)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            TournamentService.NextRun(tourney);
            _hub.CurrentContestantChanged(tournamentId);
            return this.RedirectToAction(MVC.Speaker.TournamentSpeaker.SpeakerIndex(tournamentId, null));
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "PreviousContestant")]
        public virtual ActionResult PreviousContestant(long tournamentId)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            TournamentService.PreviousRun(tourney);
            _hub.CurrentContestantChanged(tournamentId);
            return this.RedirectToAction(MVC.Speaker.TournamentSpeaker.SpeakerIndex(tournamentId, null));
        }

        [HttpPost]
        public virtual ActionResult EndRound(long tournamentId)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var result = tourney.CanEndCurrentRound();
            if (result.IsTrue)
            {
                TournamentService.EndCurrentRound(tourney);
                _hub.CurrentContestantChanged(tournamentId);
            }
            else
            {
                FormFeedbackHandler.SetError(HttpContext, result.Reason);
            }
            return this.RedirectToAction(MVC.Speaker.TournamentSpeaker.SpeakerIndex(tournamentId, null));
        }

        [OutputCacheAttribute(Duration = 0, NoStore = true)]
        public virtual ActionResult JudgeStatus(long tournamentId, int runNo)
        {
            if (runNo == 0) throw new ArgumentException("runNo must be set");
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            List<JudgeHasScoredTuple> list = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tourney);
            return PartialView(list);
        }

        [OutputCacheAttribute(Duration = 0, NoStore = true)]
        public virtual ActionResult RunControlPanel(long tournamentId, int roundNo)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var round = tourney.GetRoundNoGuarded(roundNo);
            var model = new SpeakerViewModel.RunControls(tourney, round);
            return PartialView(model);
        }

        [OutputCacheAttribute(Duration = 0, NoStore = true)]
        public virtual ActionResult NextContestantsList(long tournamentId, int roundNo)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var model = new SpeakerViewModel(tourney, roundNo);
            return PartialView(model);
        }
    }
}
