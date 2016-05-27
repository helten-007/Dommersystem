using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    [NotMapped]
    public class EditTournamentViewModel : _LayoutViewModel
    {
        public IList<Round> RoundList { get; set; }
        /// <summary>
        /// Used when iterating rounds
        /// </summary>
        public int RoundIx { get; set; }
        public int RoundCount { get; set; }
        public Tournament Tournament { get; set; }
        public bool IsStarted { get; set; }
        
        public EditTournamentViewModel()
        {
            RoundList = new List<Round>();
        }

        public EditTournamentViewModel(Tournament tournament) :this()
        {
            Tournament = tournament;
            IsStarted = tournament.Status != TournamentStatus.Prestart;
            if (Tournament.Rounds != null)
            {
                RoundList = tournament.Rounds.OrderBy(p => p.RoundNo).ToList();
                RoundCount = tournament.Rounds.Count;
            }
        }

        /// <summary>
        /// Assigns RoundNo to all RoundList elements based on index
        /// </summary>
        public void IndexToRoundNo()
        {
            for (int i = 0; i < RoundList.Count; i++)
            {
                RoundList[i].RoundNo = i + 1;
            }
        }
    }
}