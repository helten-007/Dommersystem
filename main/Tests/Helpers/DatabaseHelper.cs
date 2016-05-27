using NordicArenaServices;

namespace Tests.Helpers
{
    // Performing real DB operations
    public class DatabaseHelper
    {
        public static void DeleteTournament(long? id)
        {
            if (id != null)
            {
                using (var db = new EfTournamentService())
                {
                    db.DeleteTournament(id.Value);
                }
            }
        }
    }
}
