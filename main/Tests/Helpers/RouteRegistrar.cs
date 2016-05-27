using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using NordicArenaTournament.Areas.Admin;
using NordicArenaTournament.Areas.Judge;
using NordicArenaTournament.Areas.Speaker;

namespace Tests.Helpers
{
    internal class RouteRegistrar
    {
        /// <summary>
        /// Remember to add new areas if new areas added to solution
        /// </summary>
        /// <param name="routes"></param>
        internal static void RegisterAll(RouteCollection routes)
        {      
            new AdminAreaRegistration().RegisterArea(new AreaRegistrationContext("Admin", routes));
            new JudgeAreaRegistration().RegisterArea(new AreaRegistrationContext("Judge", routes));
            new SpeakerAreaRegistration().RegisterArea(new AreaRegistrationContext("Speaker", routes));
            NordicArenaTournament.RouteConfig.RegisterRoutes(routes);        
        }
    }
}
