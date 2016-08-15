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
namespace NordicArenaTournament.Areas.Judge.Controllers
{
    public partial class TournamentJudgeController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected TournamentJudgeController(Dummy d) { }

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

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult JudgeIndex()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeIndex);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult HeadJudgeIndex()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HeadJudgeIndex);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult JudgeIndexContent()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeIndexContent);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult JudgementList()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgementList);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult JudgeStatus()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeStatus);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult ClosestContestants()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ClosestContestants);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TournamentJudgeController Actions { get { return MVC.Judge.TournamentJudge; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Judge";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "TournamentJudge";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "TournamentJudge";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string JudgeIndex = "JudgeIndex";
            public readonly string HeadJudgeIndex = "HeadJudgeIndex";
            public readonly string JudgeIndexContent = "JudgeIndexContent";
            public readonly string JudgementList = "JudgementList";
            public readonly string JudgeStatus = "JudgeStatus";
            public readonly string ClosestContestants = "ClosestContestants";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string JudgeIndex = "JudgeIndex";
            public const string HeadJudgeIndex = "HeadJudgeIndex";
            public const string JudgeIndexContent = "JudgeIndexContent";
            public const string JudgementList = "JudgementList";
            public const string JudgeStatus = "JudgeStatus";
            public const string ClosestContestants = "ClosestContestants";
        }


        static readonly ActionParamsClass_JudgeIndex s_params_JudgeIndex = new ActionParamsClass_JudgeIndex();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_JudgeIndex JudgeIndexParams { get { return s_params_JudgeIndex; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_JudgeIndex
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string judgeId = "judgeId";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_HeadJudgeIndex s_params_HeadJudgeIndex = new ActionParamsClass_HeadJudgeIndex();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_HeadJudgeIndex HeadJudgeIndexParams { get { return s_params_HeadJudgeIndex; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_HeadJudgeIndex
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string judgeId = "judgeId";
        }
        static readonly ActionParamsClass_JudgeIndexContent s_params_JudgeIndexContent = new ActionParamsClass_JudgeIndexContent();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_JudgeIndexContent JudgeIndexContentParams { get { return s_params_JudgeIndexContent; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_JudgeIndexContent
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string judgeId = "judgeId";
        }
        static readonly ActionParamsClass_JudgementList s_params_JudgementList = new ActionParamsClass_JudgementList();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_JudgementList JudgementListParams { get { return s_params_JudgementList; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_JudgementList
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string roundNo = "roundNo";
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_JudgeStatus s_params_JudgeStatus = new ActionParamsClass_JudgeStatus();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_JudgeStatus JudgeStatusParams { get { return s_params_JudgeStatus; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_JudgeStatus
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string runNo = "runNo";
        }
        static readonly ActionParamsClass_ClosestContestants s_params_ClosestContestants = new ActionParamsClass_ClosestContestants();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ClosestContestants ClosestContestantsParams { get { return s_params_ClosestContestants; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ClosestContestants
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string roundNo = "roundNo";
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
                public readonly string ClosestContestants = "ClosestContestants";
                public readonly string HeadJudgeIndex = "HeadJudgeIndex";
                public readonly string HeadJudgeIndexContent = "HeadJudgeIndexContent";
                public readonly string JudgeIndex = "JudgeIndex";
                public readonly string JudgeIndexContent = "JudgeIndexContent";
                public readonly string JudgementList = "JudgementList";
                public readonly string JudgeStatus = "JudgeStatus";
            }
            public readonly string ClosestContestants = "~/Areas/Judge/Views/TournamentJudge/ClosestContestants.cshtml";
            public readonly string HeadJudgeIndex = "~/Areas/Judge/Views/TournamentJudge/HeadJudgeIndex.cshtml";
            public readonly string HeadJudgeIndexContent = "~/Areas/Judge/Views/TournamentJudge/HeadJudgeIndexContent.cshtml";
            public readonly string JudgeIndex = "~/Areas/Judge/Views/TournamentJudge/JudgeIndex.cshtml";
            public readonly string JudgeIndexContent = "~/Areas/Judge/Views/TournamentJudge/JudgeIndexContent.cshtml";
            public readonly string JudgementList = "~/Areas/Judge/Views/TournamentJudge/JudgementList.cshtml";
            public readonly string JudgeStatus = "~/Areas/Judge/Views/TournamentJudge/JudgeStatus.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_TournamentJudgeController : NordicArenaTournament.Areas.Judge.Controllers.TournamentJudgeController
    {
        public T4MVC_TournamentJudgeController() : base(Dummy.Instance) { }

        [NonAction]
        partial void JudgeIndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, long? judgeId);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgeIndex(long tournamentId, long? judgeId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeIndex);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "judgeId", judgeId);
            JudgeIndexOverride(callInfo, tournamentId, judgeId);
            return callInfo;
        }

        [NonAction]
        partial void HeadJudgeIndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, long? judgeId);

        [NonAction]
        public override System.Web.Mvc.ActionResult HeadJudgeIndex(long tournamentId, long? judgeId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.HeadJudgeIndex);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "judgeId", judgeId);
            HeadJudgeIndexOverride(callInfo, tournamentId, judgeId);
            return callInfo;
        }

        [NonAction]
        partial void JudgeIndexContentOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, long judgeId);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgeIndexContent(long tournamentId, long judgeId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeIndexContent);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "judgeId", judgeId);
            JudgeIndexContentOverride(callInfo, tournamentId, judgeId);
            return callInfo;
        }

        [NonAction]
        partial void JudgeIndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, NordicArenaTournament.Areas.Judge.ViewModels.JudgeViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgeIndex(NordicArenaTournament.Areas.Judge.ViewModels.JudgeViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeIndex);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            JudgeIndexOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void JudgementListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int roundNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgementList(long tournamentId, int roundNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgementList);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roundNo", roundNo);
            JudgementListOverride(callInfo, tournamentId, roundNo);
            return callInfo;
        }

        [NonAction]
        partial void JudgementListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, NordicArenaTournament.Areas.Judge.ViewModels.JudgementListViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgementList(NordicArenaTournament.Areas.Judge.ViewModels.JudgementListViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgementList);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            JudgementListOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void JudgeStatusOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int runNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult JudgeStatus(long tournamentId, int runNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeStatus);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "runNo", runNo);
            JudgeStatusOverride(callInfo, tournamentId, runNo);
            return callInfo;
        }

        [NonAction]
        partial void ClosestContestantsOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int roundNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult ClosestContestants(long tournamentId, int roundNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ClosestContestants);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roundNo", roundNo);
            ClosestContestantsOverride(callInfo, tournamentId, roundNo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
