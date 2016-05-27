using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;

namespace Tests.Common
{
    [TestClass]
    public class ExtensionsTest
    {        
        [TestMethod]
        public void GetShallowCloneByReflection_PropsAndFields()
        {
            var uri = new Uri("http://www.vg.no/");
            var source = new TestClassParent();
            source.SomePublicString = "Pu";
            source.SomePrivateString = "Pr";
            source.SomeInternalString = "I";
            source.SomeIntField = 6;
            source.SomeList = new List<Uri>() { uri };

            var dest = source.GetShallowCloneByReflection<TestClassChild>();
            source.SomePublicString = "Bye"; // Verify that the string is copied by value
            Assert.AreEqual("Pu", dest.SomePublicString);
            Assert.AreEqual("Pr", dest.SomePrivateString);
            Assert.AreEqual("I", dest.SomeInternalString);
            Assert.AreEqual(6, dest.SomeIntField);
            Assert.AreSame(source.SomeList, dest.SomeList);
            Assert.AreSame(uri, dest.SomeList[0]);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void GetShallowCloneByReflection_TournamentToJudge_NotASubclass()
        {
            var tournament = new Tournament();
            var clone = tournament.GetShallowCloneByReflection<Judge>();
        }

        [TestMethod]
        public void Shuffle_ListIsSomewhatRandomlyDistributed()
        {
            // THIS TEST MAY FAIL ALTHOUGH EVERYTHING IS OK. 
            var list = new List<String>() { "a", "b", "c" };
            var results = new List<List<String>>();
            var rng = new Random();
            for (int i = 0; i < 1000; i++)
            {
                var temp = new List<String>(list);
                temp.Shuffle(rng);
                results.Add(temp);
            }
            // Collect stats
            int[] placementOfA = new int[list.Count];
            for (int i = 0; i < 1000; i++ ) 
            {
                int ix = results[i].IndexOf("a");
                placementOfA[ix]++;
            }
            // Assert that we got a somewhat random distribution
            int minThresh = 50;
            int maxThresh = 950;
            Assert.IsTrue(placementOfA[0] >= minThresh && placementOfA[0] <= maxThresh, "pos 0: count " + placementOfA[0]);
            Assert.IsTrue(placementOfA[1] >= minThresh && placementOfA[1] <= maxThresh, "pos 1: count " + placementOfA[1]);
            Assert.IsTrue(placementOfA[2] >= minThresh && placementOfA[2] <= maxThresh, "pos 2: count " + placementOfA[2]);
            Assert.AreEqual(results.Count, placementOfA[0] + placementOfA[1] + placementOfA[2]);
        }
    }

    internal class TestClassParent
    {
        public String SomePublicString { get; set; }
        internal String SomeInternalString { get; set; }
        internal String SomePrivateString { get; set; }
        public String SomeGetOnlyString { get { return "Get"; } }
        internal List<Uri> SomeList { get; set; }
        internal int SomeIntField;
    }

    internal class TestClassChild : TestClassParent
    {
        internal String SomeChildProperty { get; set; }
    }
}
