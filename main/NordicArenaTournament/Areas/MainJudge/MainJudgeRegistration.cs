using System.Web.Mvc;

namespace NordicArenaTournament.Areas.MainJudge
{
	public class MainJudgeRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "MainJudge";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			RouteConfig.MapTournamentRoutesForArea(context, AreaName);
		}
	}
}