using System.Web.Mvc;

namespace NordicArenaTournament.Areas.Speaker
{
    public class SpeakerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Speaker";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.MapTournamentRoutesForArea(context, AreaName);
        }
    }
}
