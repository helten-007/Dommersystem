using System.Web.Mvc;

namespace NordicArenaTournament.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Public";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.MapTournamentRoutesForArea(context, AreaName);
        }
    }
}
