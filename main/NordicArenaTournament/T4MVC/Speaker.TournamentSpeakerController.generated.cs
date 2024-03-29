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
namespace NordicArenaTournament.Areas.Speaker.Controllers
{
    public partial class TournamentSpeakerController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected TournamentSpeakerController(Dummy d) { }

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
        public virtual System.Web.Mvc.ActionResult SpeakerIndex()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SpeakerIndex);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult NextContestant()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextContestant);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult PreviousContestant()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PreviousContestant);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult EndRound()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EndRound);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult JudgeStatus()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.JudgeStatus);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult RunControlPanel()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RunControlPanel);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult NextContestantsList()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextContestantsList);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public TournamentSpeakerController Actions { get { return MVC.Speaker.TournamentSpeaker; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Speaker";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "TournamentSpeaker";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "TournamentSpeaker";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string SpeakerIndex = "SpeakerIndex";
            public readonly string NextContestant = "NextContestant";
            public readonly string PreviousContestant = "PreviousContestant";
            public readonly string EndRound = "EndRound";
            public readonly string JudgeStatus = "JudgeStatus";
            public readonly string RunControlPanel = "RunControlPanel";
            public readonly string NextContestantsList = "NextContestantsList";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string SpeakerIndex = "SpeakerIndex";
            public const string NextContestant = "NextContestant";
            public const string PreviousContestant = "PreviousContestant";
            public const string EndRound = "EndRound";
            public const string JudgeStatus = "JudgeStatus";
            public const string RunControlPanel = "RunControlPanel";
            public const string NextContestantsList = "NextContestantsList";
        }


        static readonly ActionParamsClass_SpeakerIndex s_params_SpeakerIndex = new ActionParamsClass_SpeakerIndex();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SpeakerIndex SpeakerIndexParams { get { return s_params_SpeakerIndex; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SpeakerIndex
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string roundNo = "roundNo";
        }
        static readonly ActionParamsClass_NextContestant s_params_NextContestant = new ActionParamsClass_NextContestant();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_NextContestant NextContestantParams { get { return s_params_NextContestant; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_NextContestant
        {
            public readonly string tournamentId = "tournamentId";
        }
        static readonly ActionParamsClass_PreviousContestant s_params_PreviousContestant = new ActionParamsClass_PreviousContestant();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_PreviousContestant PreviousContestantParams { get { return s_params_PreviousContestant; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_PreviousContestant
        {
            public readonly string tournamentId = "tournamentId";
        }
        static readonly ActionParamsClass_EndRound s_params_EndRound = new ActionParamsClass_EndRound();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_EndRound EndRoundParams { get { return s_params_EndRound; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_EndRound
        {
            public readonly string tournamentId = "tournamentId";
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
        static readonly ActionParamsClass_RunControlPanel s_params_RunControlPanel = new ActionParamsClass_RunControlPanel();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RunControlPanel RunControlPanelParams { get { return s_params_RunControlPanel; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RunControlPanel
        {
            public readonly string tournamentId = "tournamentId";
            public readonly string roundNo = "roundNo";
        }
        static readonly ActionParamsClass_NextContestantsList s_params_NextContestantsList = new ActionParamsClass_NextContestantsList();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_NextContestantsList NextContestantsListParams { get { return s_params_NextContestantsList; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_NextContestantsList
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
                public readonly string JudgeStatus = "JudgeStatus";
                public readonly string NextContestantsList = "NextContestantsList";
                public readonly string RunControlPanel___Copy = "RunControlPanel - Copy";
                public readonly string RunControlPanel = "RunControlPanel";
                public readonly string SpeakerIndex___Copy = "SpeakerIndex - Copy";
                public readonly string SpeakerIndex = "SpeakerIndex";
                public readonly string TournamentNotRunning = "TournamentNotRunning";
            }
            public readonly string JudgeStatus = "~/Areas/Speaker/Views/TournamentSpeaker/JudgeStatus.cshtml";
            public readonly string NextContestantsList = "~/Areas/Speaker/Views/TournamentSpeaker/NextContestantsList.cshtml";
            public readonly string RunControlPanel___Copy = "~/Areas/Speaker/Views/TournamentSpeaker/RunControlPanel - Copy.cshtml";
            public readonly string RunControlPanel = "~/Areas/Speaker/Views/TournamentSpeaker/RunControlPanel.cshtml";
            public readonly string SpeakerIndex___Copy = "~/Areas/Speaker/Views/TournamentSpeaker/SpeakerIndex - Copy.cshtml";
            public readonly string SpeakerIndex = "~/Areas/Speaker/Views/TournamentSpeaker/SpeakerIndex.cshtml";
            public readonly string TournamentNotRunning = "~/Areas/Speaker/Views/TournamentSpeaker/TournamentNotRunning.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_TournamentSpeakerController : NordicArenaTournament.Areas.Speaker.Controllers.TournamentSpeakerController
    {
        public T4MVC_TournamentSpeakerController() : base(Dummy.Instance) { }

        [NonAction]
        partial void SpeakerIndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int? roundNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult SpeakerIndex(long tournamentId, int? roundNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SpeakerIndex);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roundNo", roundNo);
            SpeakerIndexOverride(callInfo, tournamentId, roundNo);
            return callInfo;
        }

        [NonAction]
        partial void NextContestantOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId);

        [NonAction]
        public override System.Web.Mvc.ActionResult NextContestant(long tournamentId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextContestant);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            NextContestantOverride(callInfo, tournamentId);
            return callInfo;
        }

        [NonAction]
        partial void PreviousContestantOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId);

        [NonAction]
        public override System.Web.Mvc.ActionResult PreviousContestant(long tournamentId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.PreviousContestant);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            PreviousContestantOverride(callInfo, tournamentId);
            return callInfo;
        }

        [NonAction]
        partial void EndRoundOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId);

        [NonAction]
        public override System.Web.Mvc.ActionResult EndRound(long tournamentId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.EndRound);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            EndRoundOverride(callInfo, tournamentId);
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
        partial void RunControlPanelOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int roundNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult RunControlPanel(long tournamentId, int roundNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RunControlPanel);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roundNo", roundNo);
            RunControlPanelOverride(callInfo, tournamentId, roundNo);
            return callInfo;
        }

        [NonAction]
        partial void NextContestantsListOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, long tournamentId, int roundNo);

        [NonAction]
        public override System.Web.Mvc.ActionResult NextContestantsList(long tournamentId, int roundNo)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.NextContestantsList);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "tournamentId", tournamentId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "roundNo", roundNo);
            NextContestantsListOverride(callInfo, tournamentId, roundNo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
