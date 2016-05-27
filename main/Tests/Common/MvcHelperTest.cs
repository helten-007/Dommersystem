using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaTournament.Common;

namespace Tests.Common
{
    [TestClass]
    public class MvcHelperTest
    {
        [TestMethod]
        public void ActionNameFor_NoArguments()
        {
            string result = MvcHelper.ActionNameFor<SomeController>(p => p.SomeAction());
            Assert.AreEqual("SomeAction", result);
        }

        [TestMethod]
        public void ActionNameFor_1Argument()
        {
            string result = MvcHelper.ActionNameFor<SomeController>(p => p.SomeAction1(1));
            Assert.AreEqual("SomeAction1", result);
        }

        [TestMethod]
        public void ActionNameFor_2Argument()
        {
            string result = MvcHelper.ActionNameFor<SomeController>(p => p.SomeAction2(0, null));
            Assert.AreEqual("SomeAction2", result);
        }

        [TestMethod]
        public void ActionNameFor_3Argument()
        {
            string result = MvcHelper.ActionNameFor<SomeController>(p => p.SomeAction3(7, "fdsa", 9));
            Assert.AreEqual("SomeAction3", result);
        }

        [TestMethod]
        public void ControllerNameFor()
        {
            string result = MvcHelper.ControllerNameFor<SomeController>();
            Assert.AreEqual("SomeController", result);
        }

        class SomeController : Controller
        {
            public ActionResult SomeAction()
            {
                return null;
            }

            public ActionResult SomeAction1(int a)
            {
                return null;
            }

            public ActionResult SomeAction2(int a, string b)
            {
                return null;
            }

            public ActionResult SomeAction3(int a, string b, long c)
            {
                return null;
            }
        }
    }
}
