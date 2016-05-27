using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;
using NordicArenaTournament.ErrorHandling;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Controllers
{
    public partial class ErrorController : LayoutControllerBase
    {
        public virtual ActionResult LoggedElmahError(ErrorLogEntry error)
        {
            return View(new LoggedElmahErrorViewModel { Error = error });
        }

        public virtual ActionResult Error404()
        {
            return View(new _LayoutViewModel());
        }

        public virtual ActionResult TestException()
        {
            throw new Exception("Testing testing @ " + DateTime.Now);
        }

        public virtual ActionResult TestAuth()
        {
            return new HttpUnauthorizedResult("Go away");
        }
    }
}
