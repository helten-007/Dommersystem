using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.Models;

namespace NordicArenaTournament.Areas.Speaker.ViewModels
{
    /// <summary>
    /// Used in speaker view for listing heats to come
    /// </summary>
    public class HeatViewModel
    {
        public int HeatNo { get; set; }
        public List<NextContestantViewModel> Contestants { get; set; }

        public HeatViewModel()
        {
            Contestants = new List<NextContestantViewModel>();
        }

        public static List<HeatViewModel> CreateListFrom(IEnumerable<RoundContestant> entries)
        {
            var set = new Dictionary<int, HeatViewModel>();
            foreach (RoundContestant entry in entries)
            {
                var heat = set.GetOrInsertNew(entry.HeatNo);
                heat.HeatNo = entry.HeatNo;
                heat.Contestants.Add(new NextContestantViewModel(entry));
            }
            return set.Values.ToList();
        }
    }
}