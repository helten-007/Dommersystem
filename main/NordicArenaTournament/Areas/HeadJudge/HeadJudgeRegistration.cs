using System.Web.Mvc;

namespace NordicArenaTournament.Areas.HeadJudge
{
	public class HeadJudgeRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "HeadJudge";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			RouteConfig.MapTournamentRoutesForArea(context, AreaName);
		}
	}
}