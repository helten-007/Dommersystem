using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaDomainModels.Resources;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Admin.ViewModels
{
    public class ContestantListAndCreate : _LayoutViewModel
    {
        public Contestant Contestant { get; set; }
        public List<Contestant> Contestants { get; set; }
        public long? IdToHighlight { get; set; }
        public bool IsRegistrationClosed { get; set; }
        public bool IsDeletionClosed { get; set; }
        public long TournamentId { get; set; }
        public String UpdateButtonText { get; set; }

        public ContestantListAndCreate()
        {
            Contestants = new List<Contestant>();
        }

        public ContestantListAndCreate(Tournament tournament, EntityMode mode) :this()
        {
            Contestants = tournament.Contestants.ToList();
            var round1 = tournament.GetRoundNo(1);
            IsRegistrationClosed = tournament.Status != TournamentStatus.Prestart &&
                                   round1.Status != TournamentStatus.Running;
            IsDeletionClosed = tournament.Status != TournamentStatus.Prestart;
            TournamentId = tournament.Id;
            Contestant = new Contestant();
            if (mode == EntityMode.Create)
            {
                UpdateButtonText = Text.Create;
            }
            else
            {
                UpdateButtonText = Text.Update;
            }
        }
    }

    public enum EntityMode
    {
        Create,
        Edit
    }
}