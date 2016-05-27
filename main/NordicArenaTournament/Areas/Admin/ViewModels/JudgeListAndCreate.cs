using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    public class JudgeListAndCreate : _LayoutViewModel
    {
        public List<NordicArenaDomainModels.Models.Judge> Judges { get; set; }
        public NordicArenaDomainModels.Models.Judge NewJudge { get; set; }
        public long? IdToHighlight { get; set; }
        public bool IsRegistrationClosed { get; set; }
        public long TournamentId { get; set; }

        public JudgeListAndCreate()
        {
            Judges = new List<NordicArenaDomainModels.Models.Judge>();
            NewJudge = new NordicArenaDomainModels.Models.Judge();            
        }

        public JudgeListAndCreate(Tournament tournament) 
        {
            IsRegistrationClosed = tournament.Status != TournamentStatus.Prestart;
            Judges = tournament.Judges.ToList();
            TournamentId = tournament.Id;
        }
    }
}