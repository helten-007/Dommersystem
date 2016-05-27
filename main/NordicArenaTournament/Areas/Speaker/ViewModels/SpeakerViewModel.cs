using System;
using System.Collections.Generic;
using System.Linq;
using NordicArenaDomainModels.Models;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Areas.Speaker.ViewModels
{
    public class SpeakerViewModel : _LayoutViewModel
    {
        public Tournament Tournament { get; set; }
        public Round Round { get;set;}
        public IList<Contestant> CurrentContestants { get; set; }
        public long CurrentContestantId { get; set; }
        public List<HeatViewModel> Heats { get; set; }
        public List<NextContestantViewModel> ContestantsFlat;
        public bool HasHeats { get; set; }
        public bool HasNextRound { get; set; }
        public bool HasPreviousRound { get; set; }
        public bool DisplayContestantList { get; set; }
        public int RoundCount { get; set; }
        public RunControls RunControlModel { get; set; }
        public List<JudgeHasScoredTuple> JudgeStatus { get; set; }
        public bool DisableStartButton { get; set; }
        public int CurrentRunNo { get; set; }
        public int CurrentHeatNo { get; set; }
        
        public SpeakerViewModel(Tournament tournament, int? roundNo = null)
        {
            var m = this;
            Round round = null;
            if (roundNo != null)
            {
                round = tournament.GetRoundNo(roundNo.Value);
                if (round == null) throw new ArgumentException(String.Format("RoundNo {0} does not exist", roundNo));
            }
            else round = tournament.GetCurrentOrDefaultRound();

            if (round == null) throw new ArgumentException("Tournament is not started or has no rounds");
            m.Tournament = tournament;
            m.Round = round;
            m.CurrentContestants = new List<Contestant>();
            m.JudgeStatus = new List<JudgeHasScoredTuple>();
            if (round.Status == TournamentStatus.Running)
            {
                m.CurrentHeatNo = tournament.GetRoundCounter().GetHeatNo();
                m.CurrentRunNo = tournament.GetRoundCounter().GetRunNo();
                m.CurrentContestants = round.GetContestantsInHeat(m.CurrentHeatNo).Select(p => p.Contestant).ToList();
                m.JudgeStatus = JudgeHasScoredTuple.GetJudgeStatusListForCurrentHeat(tournament);
            }
            m.Heats = HeatViewModel.CreateListFrom(round.GetContestantEntriesOrdered());
            m.ContestantsFlat = m.Heats.SelectMany(p => p.Contestants).ToList();
            m.DisplayContestantList = m.Round.Status != TournamentStatus.Prestart;
            m.RoundCount = m.Tournament.Rounds.Count;
            m.DisableStartButton = m.Round.Status != TournamentStatus.Running;
            m.HasNextRound = tournament.DoesRoundNoExist(round.RoundNo + 1);
            m.HasPreviousRound = tournament.DoesRoundNoExist(round.RoundNo - 1);
            m.RunControlModel = new RunControls(m.Tournament, m.Round);
            m.HasHeats = m.Heats.Any(p => p.Contestants.Count != 1);
        }

        public class RunControls
        {
            public bool EnableNextRun { get; set; }
            public bool EnablePreviousRun { get; set; }
            public int RunNo { get; set; }
            public int RunCount { get; set; }
            public int HeatNo { get; set; }
            public int HeatCount { get; set; }

            public RunControls(Tournament tournament, Round round)
            {
                var m = this;
                var running = round.Status == TournamentStatus.Running;
                m.EnableNextRun = running && tournament.HasNextRun();
                m.EnablePreviousRun = running && tournament.HasPreviousRun();
                if (running)
                {
                    var counter = tournament.GetRoundCounter();
                    m.HeatNo = counter.GetHeatNo();
                    m.RunNo = counter.GetRunNo();
                    m.HeatCount = counter.GetHeatCounter().GetMax();
                    m.RunCount = counter.GetRunCounter().GetMax();
                }
            }
        }
    }
}