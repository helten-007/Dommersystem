using System.Web.Mvc;

namespace NordicArenaTournament.Areas.Judge
{
    public class JudgeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Judge";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.MapTournamentRoutesForArea(context, AreaName);
        }
    }
}
