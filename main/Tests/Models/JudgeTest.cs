using System;
using NordicArenaDomainModels.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class JudgeTest
    {
        [TestMethod]
        public void GenerateFiveLetter()
        {
            Judge target = new Judge();
            target.GenerateLoginCode();
            Assert.AreEqual(5, target.LoginCode.Length);
            foreach (Char c in target.LoginCode) 
            {
                Assert.IsTrue(Char.IsLetterOrDigit(c));
            }
        }
    }
}
