using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.Areas.Public.Controllers
{
    public partial class TournamentPublicController : Controller
    {
        public virtual ActionResult Index()
        {
            var c = new ContentResult();
            c.Content = "Hi from public";
            return c;
        }
    }
}
