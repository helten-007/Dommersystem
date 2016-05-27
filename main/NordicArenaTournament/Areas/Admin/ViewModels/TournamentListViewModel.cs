using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    public class TournamentListViewModel : _LayoutViewModel
    {
        public IOrderedEnumerable<Tournament> Tournaments { get; set; }
    }
}