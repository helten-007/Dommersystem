using System.Data.Entity.Migrations;

// Never change this namespace. Will fuck up migration history
namespace NordicArenaTournament.Database
{
    public class NordicArenaMigrationConfig : DbMigrationsConfiguration<NordicArenaDataContext>
    {
        public NordicArenaMigrationConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(NordicArenaDataContext context)
        {
        }
    }
}