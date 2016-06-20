using System;
using System.Web.Mvc;
using Links;
using NordicArenaTournament.Areas.Admin.Controllers;
using NordicArenaTournament.Areas.Judge.Controllers;
using NordicArenaTournament.Areas.Public.Controllers;
using NordicArenaTournament.Areas.Speaker.Controllers;
using NordicArenaTournament.Common;

namespace NordicArenaTournament.ViewModels
{
    /// <summary>
    /// ViewModel for root viewModel "_Layout.cshtml"
    /// </summary>
    public class _LayoutViewModel
    {
        public bool HasFeedback { get; set; }
        public MvcHtmlString FeedbackText { get; set; }
        public string BannerImageUrl { get; set; }
        public string SponsorImageUrl1 { get; set; }
        public string SponsorImageUrl2 { get; set; }


        public _LayoutViewModel ()
        {
            try
            {
                //SponsorImageUrl1 = Content.images.sponsors.dnb_small_png;
                //SponsorImageUrl2 = Content.images.sponsors.samsung_png;
				SponsorImageUrl1 = null;
				SponsorImageUrl2 = null;
                BannerImageUrl = Content.images.banner2_jpg;
            }
            catch (TypeInitializationException ex)
            {
                // Will happen in unit tests because of T4MVC's use of VirtualPathUtility which requires HttpContext to be set.
            }
        }

        public void SetBannerImageUrlFromAction(string controller, string action)
        {
            switch (controller) {
                case TournamentAdminController.NameConst: BannerImageUrl = Content.images.banner1_jpg; break;
                case TournamentJudgeController.NameConst: BannerImageUrl = Content.images.banner2_jpg; break;
                case TournamentSpeakerController.NameConst: BannerImageUrl = Content.images.banner3_jpg; break;
                case TournamentPublicController.NameConst: BannerImageUrl = Content.images.banner4_jpg; break;
                case MainController.NameConst: BannerImageUrl = Content.images.banner5_jpg; break;
                default: BannerImageUrl = Content.images.banner6_jpg; break;
            }
        }
    }
}