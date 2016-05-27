using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Common;
using Tests.Helpers;

namespace Tests
{
    [TestClass]
    public class NaHtmlTest
    {
        [TestMethod]
        public void EnumValueFor_TournamentEnded_Avsluttet()
        {
            MvcFaker.SetLanguage("nb-NO");
            Tournament tourney = new Tournament();
            tourney.Status = TournamentStatus.Ended;
            var res = NaHtml.EnumValueFor(tourney.Status);
            Assert.AreEqual("Avsluttet", res.ToHtmlString());
        }

        /* Todo: Fix ObjecTReferenceExcepton through mocking for this test to work again 
        [TestMethod]
        public void EnumValueFor_TournamentEnded_Avsluttet()
        {
            MvcFaker.SetLanguage("nb-NO");
            Tournament tourney = new Tournament();
            tourney.Status = TournamentStatus.Ended;
            var htmlHelper = MvcFaker.CreateHtmlHelper(tourney);
            var res = htmlHelper.EnumValueFor(p => p.Status);
            Assert.AreEqual("Avsluttet", res.ToHtmlString());
        }
         * */
    }
}

