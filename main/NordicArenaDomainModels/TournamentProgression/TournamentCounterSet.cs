namespace NordicArenaDomainModels.TournamentProgression
{
    /// <summary>
    /// Round counter for a tournament. First counter is heat, second is run. So "4:2" means Heat 4, run 2
    /// </summary>
    public class TournamentCounterSet : CounterSet
    {
        public int GetHeatNo()
        {
            return GetHeatCounter().Value;
        }

        public int GetRunNo()
        {
            return GetRunCounter().Value;
        }

        public Counter GetHeatCounter()
        {
            return Counters[0];
        }

        public Counter GetRunCounter()
        {
            return Counters[1];
        }
    }
}