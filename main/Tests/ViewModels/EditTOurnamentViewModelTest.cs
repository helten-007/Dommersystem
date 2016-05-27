using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.Areas.Admin.ViewModels;

namespace Tests.ViewModels
{
    [TestClass]
    public class EditTournamentViewModelTest
    {
        [TestMethod]
        public void CreateShallowCopyOf_OK()
        {
            var round = new Round() { Id = 2 };
            var tournament = new Tournament();
            tournament.Name = "Roger";
            tournament.Id = 1;
            tournament.Rounds = new HashSet<Round>();
            tournament.Rounds.Add(round);

            var tmodel = new EditTournamentViewModel(tournament);
            Assert.AreEqual("Roger", tmodel.Tournament.Name);
            Assert.AreEqual(1, tmodel.Tournament.Id);
            Assert.AreSame(tournament.Rounds, tmodel.Tournament.Rounds);
            Assert.AreSame(tournament.Rounds.ToList().FirstOrDefault(), tmodel.Tournament.Rounds.ToList().FirstOrDefault());
            Assert.AreEqual(tournament.Rounds.ToList().FirstOrDefault(), tmodel.RoundList[0]);
        }
    }
}
