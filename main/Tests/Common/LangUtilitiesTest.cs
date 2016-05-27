using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Lang;
using NordicArenaTournament.Common;

namespace Tests.Common
{
    [TestClass]
    public class LangUtilitiesTest
    {
        [TestMethod]
        public void MethodNameForNonGeneric_Action()
        {
            String result = LangUtilities.MethodNameFor(() => new LangUtilitiesTest().SomeMethod(null, null));
            Assert.AreEqual("SomeMethod", result);
        }

        [TestMethod]
        public void MethodNameForNonGeneric_Func1()
        {
            String result = LangUtilities.MethodNameFor(() => new LangUtilitiesTest().SomeFunc1(0));
            Assert.AreEqual("SomeFunc1", result);
        }

        [TestMethod]
        public void MethodNameForGeneric_Func()
        {
            String result = LangUtilities.MethodNameFor<LangUtilitiesTest, int>(p => p.SomeFunc0());
            Assert.AreEqual("SomeFunc0", result);
        }

        [TestMethod]
        public void GetDecimalCount_0()
        {
            decimal d = 14M;
            var result = LangUtilities.GetDecimalCount(d);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void GetDecimalCount_1()
        {
            decimal d = 14.5M;
            var result = LangUtilities.GetDecimalCount(d);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetDecimalCount_10()
        {
            decimal d = 173.01234567890M;
            var result = LangUtilities.GetDecimalCount(d);
            Assert.AreEqual(10, result);
        }

        private void SomeMethod(object sender, EventArgs e)
        {
        }

        private int SomeFunc0()
        {
            return 0;
        }

        private int SomeFunc1(int a)
        {
            return 0;
        }

        [TestMethod]
        public void AreInRisingOrder_Yes1()
        {
            var list = new List<int> {1,2,3};
            Assert.IsTrue(list.AreInRisingOrder(p => p));
        }

        [TestMethod]
        public void AreInRisingOrder_Yes2()
        {
            var list = new List<int?> {1,3,3};
            Assert.IsTrue(list.AreInRisingOrder(p => p.Value));
        }
        [TestMethod]
        public void AreInRisingOrder_Float_Yes()
        {
            var list = new List<float> { 1.0F, 1.0F, 1.1F };
            Assert.IsTrue(list.AreInRisingOrder(p => p));
        }

        [TestMethod]
        public void AreInRisingOrder_Float_No()
        {
            var list = new List<float> {1F, 0F, 5F};
            Assert.IsFalse(list.AreInRisingOrder(p => p));
        }
    }
}
