using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using ModelMetadataExtensions;
using NordicArenaDomainModels.Resources;
using NordicArenaTournament.Controllers;
using NordicArenaTournament.Database;
using NordicArenaTournament.ErrorHandling;

namespace NordicArenaTournament
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Ensure Datamodel is updated if classes changed. Learn more: http://stackoverflow.com/questions/13238203/
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<NordicArenaDataContext, NordicArenaMigrationConfig>());

            // Binding of properties to display name by convention:
            // http://haacked.com/archive/2011/07/14/model-metadata-and-validation-localization-using-conventions.aspx/
            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(
                requireConventionAttribute: false,
                defaultResourceType: typeof(PropertyNames)
            );
        }

        // Invoked by Elmah. http://stackoverflow.com/a/2049364/507339
        void ErrorLog_Logged(object sender, ErrorLoggedEventArgs e)                
        {   
            Trace.TraceError("Logged ELMAH error with ID: " + e.Entry.Id);
            if (e.Entry.Error.StatusCode == 0 || e.Entry.Error.StatusCode == 500)
            {
                IController errorController = new ErrorController();
                var routeData = new RouteData();
                routeData.Values["action"] = "LoggedElmahError";
                routeData.Values["controller"] = "Error";
                routeData.Values["error"] = e.Entry;
                try
                {
                    // Set the tournament Id for convenience. MAkes the navigation menu still work in context of a tournament
                    routeData.Values["tournamentId"] =
                        HttpContext.Current.Request.RequestContext.RouteData.Values["tournamentId"];
                }
                catch (Exception) { }
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.StatusCode = 500;
                Response.End();
            }
        }
    }
}