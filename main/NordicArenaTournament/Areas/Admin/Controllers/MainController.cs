using System.Linq;
using System.Web.Mvc;
using NordicArenaDomainModels.Models;
using NordicArenaServices;
using NordicArenaTournament.Areas.Admin.ViewModels;
using NordicArenaTournament.Common;
using NordicArenaTournament.Controllers;

namespace NordicArenaTournament.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for pages with no Tournament-context
    /// </summary>
    public partial class MainController : TournamentControllerBase
    {
        public MainController() { }

        public MainController(FormFeedbackHandler formFeedbackHandler, ITournamentService tournamentService) 
            : base(formFeedbackHandler, tournamentService)
        {
            
        }

        public virtual ActionResult Index()
        {
            return this.RedirectToAction(MVC.Admin.Main.ActionNames.TournamentList);
        }

        public virtual ActionResult TournamentList()
        {
            var model = new TournamentListViewModel();
            model.Tournaments = TournamentService.GetTournaments().OrderByDescending(p => p.Id);
            return View(model);
        }

        public virtual ActionResult CreateTournament()
        {
            var model = new EditTournamentViewModel(new Tournament());
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CreateTournament(Tournament tournament)
        {
            tournament.InitializeDefaults();
            TournamentService.AddTournament(tournament);
            return RedirectToRoute(new { action = MVC.Admin.TournamentAdmin.ActionNames.EditTournament, tournamentId = tournament.Id });
        }
    }
}
