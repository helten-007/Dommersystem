using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NordicArenaDomainModels.Common;
using NordicArenaDomainModels.Helpers;
using NordicArenaDomainModels.Interfaces;
using NordicArenaDomainModels.Lang;
using NordicArenaDomainModels.ObjectValidation;
using NordicArenaDomainModels.TournamentProgression;

namespace NordicArenaDomainModels.Models
{
    /// <summary>
    /// Represents a tournament
    /// </summary>
    public class Tournament
    {
        public long Id { get; set; }
        [Required]
        public String Name { get; set; }
        public TournamentStatus Status { get; set; }

		/// <summary>
		/// Number of contestants visible on the scoreboard
		/// </summary>
		public int? ContestantsOnScoreboard { get; set; }

        /// <summary>
        /// Serialization of CounterSet
        /// </summary>
        public String CurrentRun { get; set; }
        /// <summary>
        /// Indicator which signals when judges can start submitting scores
        /// </summary>
        public bool IsCurrentRunDone { get; set; }
        /// <summary>
        /// True if system should shuffle the player list before assigning heats before
        /// each round
        /// </summary>
        public bool ShufflePlayerList { get; set; }

        public virtual ICollection<Contestant> Contestants { get; set; }
        public virtual ICollection<Judge> Judges { get; set; }
        [ValidateObject] 
        public virtual ICollection<Round> Rounds { get; set; }
        [ValidateObject]
        public virtual ICollection<JudgingCriterion> JudgingCriteria { get; set; }

        private ResultReasonTuple _canEndRound; // Caching this value as it is expensive to calculate        

          
        /// <summary>
        /// Returns current contestant, if tournament is on format where that is enabled
        /// </summary>
        public RoundContestant GetCurrentRoundContestant()
        {
            var rc = GetRoundCounter() as RoundIndividualCounterSet;
            if (rc == null) return null;
            int contNo = rc.GetContesantNo();
            var heatNo = rc.GetHeatNo();
            var round = GetCurrentRound();
            var heatContestants = round.GetContestantEntriesOrdered().Where(p => p.HeatNo == heatNo);
            return heatContestants.ToList()[contNo - 1];
        }

        private TournamentCounterSet _roundCounter;
        /// <summary>
        /// Counter for Heat:RunNo:ContestantNo
        /// </summary>
        /// <returns></returns>
        public TournamentCounterSet GetRoundCounter()
        {
            if (_roundCounter == null) InitCounterSet();            
            return _roundCounter;
        }

        /// <summary>
        /// Restes the counter to 1:1:1
        /// </summary>
        private void ResetCounterSet()
        {
            CurrentRun = null;
            InitCounterSet();
        }

        /// <summary>
        /// Loads counter set from CurrentRun, or resets if blank
        /// </summary>
        private void InitCounterSet()
        {
            var currentRound = GetCurrentRound();
            _roundCounter = RoundHeatCounterSet.CreateCounterSetFor(currentRound);
            if (CurrentRun != null) _roundCounter.SetValuesFromString(CurrentRun);
            CurrentRun = _roundCounter.ToString();
        }

        internal void SetRoundCounter(int heat, int run)
        {
            var rc = GetRoundCounter();
            rc.SetValue(heat, run);
            SetRoundCounter(rc);
        }

        internal void SetRoundCounter(TournamentCounterSet counter)
        {
            _roundCounter = counter;
            CurrentRun = _roundCounter.ToString();
        }

        public Tournament() 
        {
            Status = TournamentStatus.Prestart;
        }

        public Tournament(String name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initializing lists. Not doing this in constructor because EF creates proxied lists. Only use when adding new or unit testing
        /// </summary>
        public void EnsureListsAreInitialized()
        {
            if (Rounds == null) Rounds = new HashSet<Round>();
            if (Judges == null) Judges = new HashSet<Judge>();
            if (Contestants == null) Contestants = new HashSet<Contestant>();
        }

        /// <summary>
        /// Sets up the default tournament parameters. Creates a first round and sets default judging criterais
        /// </summary>
        public void InitializeDefaults()
        {
            EnsureListsAreInitialized();
            Rounds.Clear();
            JudgingCriteria = DefaultDataFactory.GetJudgingCriteria();
            foreach (var crit in JudgingCriteria) { crit.Tournament = this; }
            // Adds a single round called "Finale"
            var round = new Round(Resources.Text.Finals);
            round.Tournament = this;
            round.RoundNo = 1;
            Rounds.Add(round);
        }

        /// <summary>
        /// Assigns QualifiesFrom-pointers based on RoundNo
        /// </summary>
        public void AssignRoundQualifyPointers()
        {
            var list = Rounds.OrderBy(p => p.RoundNo).ToList();
            Round prevRound = null;
            for (int i = 0; i < list.Count; i++)
            {
                var round = list[i];
                round.QualifiesFromRound = prevRound;
                prevRound = round;
            }
        }

        /// <summary>
        /// Returns the rounds in Round not in list roundList based in Id
        /// </summary>
        /// <param name="roundList"></param>
        /// <returns></returns>
        public List<Round> GetRoundsNotInList(IEnumerable<Round> roundList)
        {
            var idList = roundList.Select(p => p.Id).ToList();
            return Rounds.Where(p => !idList.Contains(p.Id)).ToList();
        }

        public RoundContestant AddContesantToRound(Contestant contestant, Round round)
        {
            return round.AddContesant(contestant);
        }

        /// <summary>
        /// Returns true if the tournament is ready to be started
        /// </summary>
        /// <param name="result">Result object with error messages</param>
        /// <returns></returns>
        public bool CanBeStarted(out ValidationResultSet result)
        {
            result = ValidationHelper.Validate(this);
            if (Status != TournamentStatus.Prestart) result.Results.Add(new ValidationResult(Resources.Text.TournamentAlreadyStarted));
            if (Contestants.Count == 0) result.Results.Add(new ValidationResult(Resources.Text.TournamentTooFewContestants));
            if (Judges.Count == 0) result.Results.Add(new ValidationResult(Resources.Text.TournamentNoJudges));
            result.IsValid = result.Results.Count == 0;
            return result.IsValid;
        }

        /// <summary>
        /// Returns true if the tournament is ready to be started
        /// </summary>
        public bool CanBeStarted()
        {
            var res = new ValidationResultSet();
            return CanBeStarted(out res);
        } 

        /// <summary>
        /// Starts the tournament 
        /// </summary>
        /// <param name="contestantListSorter">An optional implementation of a list sorting algorithm. Defaults to random player shuffeling.</param>
        public void Start(IListSorter contestantListSorter = null)
        {
            if (contestantListSorter == null) contestantListSorter = GetListSorter();
            var round1 = Rounds.FirstOrDefault(p => p.RoundNo == 1);
            round1.Start(contestantListSorter, Contestants);
            // Set current contestant and tournament status
            Status = TournamentStatus.Running;
            ResetCounterSet();
        }


        /// <summary>
        /// Advances current round to next run. Advances to next player if max runs is reached
        /// </summary>
        /// <returns>true if not end of round</returns>
        public void NextRun()
        {
            GetRoundCounter().Inc();
            IsCurrentRunDone = false;
            CurrentRun = GetRoundCounter().ToString();
        }

        /// <summary>
        /// Backs up the tournament one run and backs up current player as well if needed
        /// </summary>
        /// <returns></returns>
        public void PreviousRun()
        {
            GetRoundCounter().Dec();
            IsCurrentRunDone = false;
            CurrentRun = GetRoundCounter().ToString();
        }

        public bool HasNextRun()
        {
            return !GetRoundCounter().IsAtMax();
        }

        public bool HasPreviousRun()
        {
            return !GetRoundCounter().IsAtMin();
        }

        public IOrderedEnumerable<Round> GetRoundsOrdered()
        {
            return Rounds.OrderBy(p => p.RoundNo);
        }

        /// <summary>
        /// Returns the current running round. Null if ended or not started
        /// </summary>
        /// <returns></returns>
        public Round GetCurrentRound()
        {
            var round = GetRoundsOrdered().SkipWhile(p => p.Status == TournamentStatus.Ended).FirstOrDefault(); // First prestart or running
            return round;
        }

        /// <summary>
        /// Returns first round with the specified round number
        /// </summary>
        public Round GetRoundNo(int roundNo)
        {
            return Rounds.FirstOrDefault(p => p.RoundNo == roundNo);
        }

        /// <summary>
        /// Returns first round with the specified round number
        /// </summary>
        public Round GetRoundNoGuarded(int roundNo)
        {
            var round = GetRoundNo(roundNo);
            if (round == null) throw new ArgumentException(String.Format("Could not find round no {0}", roundNo));
            return round;
        }

        public Round GetNextRound(Round currentRound = null)
        {
            if (currentRound == null) currentRound = GetCurrentRound();
            if (currentRound == null) return GetRoundsOrdered().FirstOrDefault();
            return Rounds.FirstOrDefault(p => currentRound.Equals(p.QualifiesFromRound));
        }

        public Round GetPreviousRound()
        {
            var current = GetCurrentRound();
            if (current == null) return Rounds.LastOrDefault(p => p.Status == TournamentStatus.Ended);
            return current.QualifiesFromRound;
        }

        /// <summary>
        /// True if there is planned a next round after the current active round
        /// </summary>
        public bool HasNextRound()
        {
            return GetNextRound() != null;
        }

        public bool HasPreviousRound()
        {
            var round = GetPreviousRound();
            return round != null;
        }

        public bool DoesRoundNoExist(int roundNo)
        {
            return Rounds.Any(p => p.RoundNo == roundNo);
        }

        /// <summary>
        /// Returns first running round or the last round in the tournament if tournament ended
        /// </summary>
        /// <returns></returns>
        public Round GetCurrentOrDefaultRound()
        {
            var round = GetCurrentRound();
            if (round == null) round = Rounds.FirstOrDefault(p => p.Status == TournamentStatus.Prestart);
            if (round == null) round = Rounds.LastOrDefault(p => p.Status == TournamentStatus.Ended);
            return round;        
        }

        public ResultReasonTuple CanEndCurrentRound(bool? forceRecheck = false)
        {
            if (_canEndRound == null || forceRecheck.IsTrue())
            {
                _canEndRound = new ResultReasonTuple();
                var round = GetCurrentRound();
                if (round != null) _canEndRound = round.CanBeEnded();
            }
            return _canEndRound;
        }

        /// <summary>
        /// Ends the round if in running state. Advanced tournament to next round
        /// </summary>
        /// <returns></returns>
        public ResultReasonTuple EndCurrentRound()
        {
            var result = CanEndCurrentRound();
            if (result.IsTrue)
            {
                var curRound = GetCurrentRound();
                curRound.End();
                var nextRound = GetRoundNo(curRound.RoundNo + 1);
                if (nextRound == null) // Tournament over?
                {                    
                    Status = TournamentStatus.Ended; 
                }
                else
                {
                    var listSorter = GetListSorter();
                    nextRound.Start(listSorter);
                    ResetCounterSet();
                }
            }
            return result;
        }

        private IListSorter GetListSorter()
        {
            if (ShufflePlayerList) return new RandomizedListSorter();
            return new PassThroughListSorter();
        }

        /// <summary>
        /// Returns the number of expected judgement rows per contestant per run, upon judgement completion.
        /// </summary>x
        public virtual int GetExpectedJudgementCountPerRun()
        {
            // Guards
            if (Judges == null) throw new ArgumentException("Judges navigation property not set");
            if (JudgingCriteria == null) throw new ArgumentException("JudgingCriteria navigation property not set");
            // Logic
            int judgeCount = Judges.Count();
            int criteriaCount = JudgingCriteria.Count();
            return judgeCount * criteriaCount;
        }

        internal int GetCurrentRunNo()
        {
            return GetRoundCounter().GetRunNo();
        }

        /// <summary>
        /// Restes the current round. Effectively sets CurrentRound -= 1
        /// </summary>
        /// <param name="roundNo"></param>
        public List<Object> ResetRound(int roundNo)
        {
            var deletedObjects = new List<Object>();
            if (roundNo < Rounds.Count) deletedObjects.AddRange(ResetRound(roundNo + 1));
            var round = GetRoundNoGuarded(roundNo);
            var deltedObjectsInRound = round.Reset();
            deletedObjects.AddRange(deltedObjectsInRound);
            if (roundNo > 1) // Update status of previous round?
            {
                var prevRound = GetRoundNoGuarded(roundNo - 1);
                prevRound.Status = TournamentStatus.Running;
            }
            
            // Reset round counter
            if (roundNo > 1)
            {
                GetRoundCounter().GetHeatCounter().Value = 1;
                GetRoundCounter().GetRunCounter().Value = 1;
                CurrentRun = GetRoundCounter().ToString();
            }
            else Status = TournamentStatus.Prestart; // Reset tournament status?
            return deletedObjects;
        }
    }

    public enum TournamentStatus
    {
        /// <summary>
        /// Tournament is in configuration / setup phase
        /// </summary>
        Prestart = 0,
        /// <summary>
        /// Tournament is running
        /// </summary>
        Running = 1,
        /// <summary>
        /// Tournament is ended
        /// </summary>
        Ended = 2
    }
}
