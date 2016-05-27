using System;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;
using NordicArenaTournament;
using NordicArenaTournament.Areas.Admin.Controllers;
using NordicArenaTournament.Areas.Judge.Controllers;
using NordicArenaTournament.Controllers;
using Tests.Helpers;

namespace Tests
{
    [TestClass]
    public class RouteTest
    {
        [TestMethod]
        public void RouteTest_AdminTournamentDefaultPageNew()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Admin/Tournament/14").To<TournamentAdminController>(x => x.AdminIndex(14));
        }

        [TestMethod]
        public void RouteTest_AdminTournamentJudges()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Admin/Tournament/14/JudgeList").To<TournamentAdminController>(x => x.JudgeList(14));
        }

        [TestMethod]
        public void RouteTest_Admin()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Admin").To<MainController>(x => x.Index());
        }

        [TestMethod]
        public void RouteTest_AdminMain()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Admin/Main").To<MainController>(x => x.Index());
        }

        [TestMethod]
        public void RouteTest_DefaultRoute()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/").To<RootController>(x => x.Index());
        }

        [TestMethod]
        public void RouteTest_Error404()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Error/Error404").To<ErrorController>(x => x.Error404());
        }

        [TestMethod]
        public void RouteTest_JudgeIndex()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/Judge/Tournament/1").To<TournamentJudgeController>(x => x.JudgeIndex(1, null));
        }

        [TestMethod]
        public void RouteTest_Elmah_NoRoute()
        {
            var routes = new RouteCollection();
            RouteRegistrar.RegisterAll(routes);
            routes.ShouldMap("~/elmah.axd").ToIgnoredRoute();
        }
    }
}
