using System.Web.Mvc;
using Microsoft.Practices.Unity;
using NordicArenaTournament.Common;
using NordicArenaTournament.ViewModels;

namespace NordicArenaTournament.Controllers
{
    /// <summary>
    /// Base class for all controllers which presents a view dependent on _Layout.cshtml
    /// </summary>
    public class LayoutControllerBase : Controller
    {
        [Dependency]
        protected FormFeedbackHandler FormFeedbackHandler { get; set; }

        public LayoutControllerBase() {} // Need the empty constructor for T4MVC to work. Therefore also the DepencyAttribute

        public LayoutControllerBase(FormFeedbackHandler formFeedbackHandler)
        {
            FormFeedbackHandler = formFeedbackHandler;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var viewModel = ViewData.Model as _LayoutViewModel;
            if (viewModel != null && FormFeedbackHandler != null)
            {
                viewModel.HasFeedback = FormFeedbackHandler.HasFeedback();
                viewModel.FeedbackText = FormFeedbackHandler.GetFeedbackHtml();
            }
            base.OnActionExecuted(filterContext);
        }
    }
}