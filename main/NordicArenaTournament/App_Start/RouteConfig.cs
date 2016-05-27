using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NordicArenaTournament
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "DefaultRoot",
                url: "{controller}/{action}",
                defaults: new { controller = "Root", action = "Index" }
            );
        }

        /// <summary>
        /// Maps default routes for tournament-context pages in the area
        /// </summary>
        /// <param name="AreaName">Name of the area</param>
        /// <param name="controllerName">Controller name (optional). If none set, a combination of "Tournament" + AreaName will be used.</param>
        internal static void MapTournamentRoutesForArea(AreaRegistrationContext context, string AreaName, string controllerName = null)
        {
            if (controllerName == null) controllerName = "Tournament" + AreaName;
            context.MapRoute(
                AreaName + "_tournament",
                AreaName + "/Tournament/{tournamentId}/{action}/{id}",
                new { controller = controllerName, tournamentId = "-1", action = AreaName+"Index", id = UrlParameter.Optional }
            );
        }
    }
}