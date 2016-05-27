using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.Controllers
{
    public partial class RootController : LayoutControllerBase
    {
        public virtual ActionResult Index()
        {
            return RedirectToRoute(new { area = "Admin" ,controller = "" });
        }
    }
}
