using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NordicArenaDomainModels.Lang;

namespace NordicArenaTournament.Common
{
    /// <summary>
    /// Static methods empowering the ASP.NET MVC framework
    /// </summary>
    public static class MvcHelper
    {
        #region Context-independent helper methods
        public static MvcHtmlString DisableIf(this HtmlHelper html, bool condition)
        {
            if (condition) return new MvcHtmlString("disabled=\"disabled\"");
            return new MvcHtmlString(String.Empty);
        }
        #endregion

        #region HtmlHelper-specific
        /// <summary>
        /// Returns an object with the property "class" set to classname, if current ACTION==action
        /// </summary>
        public static object ClassIfAtAction(this HtmlHelper html, String className, String action)
        {
            return ClassIfTrue(html, className, html.CurrentAction() == action);
        }
        /// <summary>
        /// Returns an object with the property "class" set to classname, if current ACTION==action
        /// </summary>
        public static object ClassIfTrue(this HtmlHelper html, String className, bool condition)
        {
            if (condition) return new { @class = className };
            return new object();
        }

        public static String CurrentController(this HtmlHelper html)
        {
            return html.ViewContext.RouteData.GetControllerName();
        }

        public static String CurrentAction(this HtmlHelper html)
        {
            return html.ViewContext.RouteData.GetActionName();
        }

        public static String ActionNameFor<T>(Expression<Func<T, ActionResult>> actionExpression) where T : Controller
        {
            return LangUtilities.MethodNameFor(actionExpression);
        }

        public static String ControllerNameFor<T>() where T : Controller
        {
            return typeof(T).Name;
        }

        public static String GetActionName(this RouteData routingData)
        {
            return routingData.Values["action"].ToString();
        }

        public static String GetControllerName(this RouteData routingData)
        {
            return routingData.Values["controller"].ToString();
        }
        #endregion
    }
}