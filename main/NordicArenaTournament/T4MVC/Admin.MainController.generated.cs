// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace NordicArenaTournament.Areas.Admin.Controllers
{
    public partial class MainController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected MainController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }


        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public MainController Actions { get { return MVC.Admin.Main; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Admin";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Main";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Main";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string TournamentList = "TournamentList";
            public readonly string CreateTournament = "CreateTournament";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string TournamentList = "TournamentList";
            public const string CreateTournament = "CreateTournament";
        }


        static readonly ActionParamsClass_CreateTournament s_params_CreateTournament = new ActionParamsClass_CreateTournament();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_CreateTournament CreateTournamentParams { get { return s_params_CreateTournament; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_CreateTournament
        {
            public readonly string tournament = "tournament";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string CreateTournament = "CreateTournament";
                public readonly string TournamentList = "TournamentList";
            }
            public readonly string CreateTournament = "~/Areas/Admin/Views/Main/CreateTournament.cshtml";
            public readonly string TournamentList = "~/Areas/Admin/Views/Main/TournamentList.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_MainController : NordicArenaTournament.Areas.Admin.Controllers.MainController
    {
        public T4MVC_MainController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void TournamentListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult TournamentList()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.TournamentList);
            TournamentListOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateTournamentOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult CreateTournament()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateTournament);
            CreateTournamentOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void CreateTournamentOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, NordicArenaDomainModels.Models.Tournament tournament);

        [NonAction]
        public override System.Web.Mvc.ActionResult CreateTournament(NordicArenaDomainModels.Models.Tournament tournament)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.CreateTournament);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournament", tournament);
            CreateTournamentOverride(callInfo, tournament);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591