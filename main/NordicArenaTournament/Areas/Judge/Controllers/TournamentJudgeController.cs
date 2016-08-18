using System.Web.Mvc;
using NordicArenaServices;
using NordicArenaTournament.Areas.Judge.ViewModels;
using NordicArenaTournament.Common;
using NordicArenaTournament.Controllers;
using NordicArenaTournament.SignalR;
using System;
using System.Collections.Generic;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Speaker.ViewModels;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Judge.Controllers
{
    public partial class TournamentJudgeController : TournamentControllerBase
    {
        private NaHub _hub;

        public TournamentJudgeController(FormFeedbackHandler formFeedbackHandler, ITournamentService tournamentService, NaHub naHub) : base(formFeedbackHandler, tournamentService)
        {
            _hub = naHub;
        }

        public virtual ActionResult JudgeIndex(long tournamentId, long? judgeId = null)
        {
            if (judgeId == null) return this.RedirectToAction(MVC.Admin.TournamentAdmin.JudgeList(tournamentId));
            var model = GetModelForJudgeIndex(tournamentId, judgeId);
            return View(model);
        }

		public virtual ActionResult HeadJudgeIndex(long tournamentId, long? judgeId = null)
		{
			if (judgeId == null) return this.RedirectToAction(MVC.Admin.TournamentAdmin.JudgeList(tournamentId));
			var model = GetModelForHeadJudge(tournamentId, judgeId);
			return View(model);
		}

		private HeadJudgeViewModel GetModelForHeadJudge(long tournamentId, long? judgeId = null)
		{
			var tourney = TournamentService.GetTournamentGuarded(tournamentId);
			var judges = tourney.Judges;

			foreach (var j in judges)
			{
				if (j.Id == judgeId)
				{
					judges.Remove(j);
					break;
				}
			}
			var judge = TournamentService.GetJudgeGuarded(judgeId.Value);
			var model = new HeadJudgeViewModel(tourney, judge, judges);
			return model;
		}

        public virtual ActionResult JudgeIndexContent(long tournamentId, long judgeId)
        {
            var model = GetModelForJudgeIndex(tournamentId, judgeId);
            return PartialView(model);
        }

        private JudgeViewModel GetModelForJudgeIndex(long tournamentId, long? judgeId = null)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var judge = TournamentService.GetJudgeGuarded(judgeId.Value);
            var model = new JudgeViewModel(tourney, judge);
            return model;
        }

        /// <summary>
        /// Posting of judgement scores from a run from a single judge 
        /// </summary>
        [HttpPost]
        public virtual ActionResult JudgeIndex(JudgeViewModel model)
        {
            foreach (var contestantModel in model.Contestants)
            {
                TournamentService.ReplaceRunJudgings(model.Tournament.Id, contestantModel.RoundContestantId, contestantModel.Scores);
            }

			if (model.Judge.IsHeadJudge)
				_hub.HeadJudgeScoreSubmitted(model.Tournament.Id);
            _hub.JudgeScoresSubmitted(model.Tournament.Id);
            return new ContentResult();
        }

        public virtual ActionResult JudgementList(long tournamentId, int roundNo = 1)
        {
            var tourney = TournamentService.GetTournamentGuarded(tournamentId);
            var model = new JudgementListViewModel(tourney, roundNo);
            return View(model);
        }

        /// <summary>
        /// Posting of judgement scores from arbitrary judges/contestants. Used in JudgementList / "Karakterer". 
        /// </summary>
        [HttpPost]
        public virtual ActionResult JudgementList(JudgementListViewModel model)
        {
            var tourney = TournamentService.GetTournamentGuarded(model.TournamentId);
            var judgements = model.GetRunJudgings(tourney);
            TournamentService.ReplaceAllRunJudgings(judgements, tourney);
            return RedirectToAction(MVC.Judge.TournamentJudge.JudgementList(model.TournamentId, model.RoundNo));
        }

		[OutputCacheAttribute(Duration = 0, NoStore = true)]
		public virtual ActionResult JudgeStatus(long tournamentId, int runNo)
		{
			if (runNo == 0) throw new ArgumentException("runNo must be set");
			var tourney = TournamentService.GetTournamentGuarded(tournamentId);
			List<JudgeHasScoredTuple> list = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tourney);
			return PartialView(list);
		}

		public virtual ActionResult ClosestContestants(long tournamentId, int roundNo = 1)
		{
			var tourney = TournamentService.GetTournamentGuarded(tournamentId);
			var model = new ClosestContestantsViewModel(tourney, roundNo);
			return PartialView(model);
		}
    }
}

