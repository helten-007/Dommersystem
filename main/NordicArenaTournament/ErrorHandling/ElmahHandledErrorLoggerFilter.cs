using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace NordicArenaTournament.ErrorHandling
{
    /// <summary>
    /// Class is needed if/when HandleErrorAttribute-filter is added in FilterConfig. 
    /// The HandleError filter would then swallow 500-errors, so we'd need this filter to re-enable Elmah to log them.
    /// </summary>
    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Log only handled exceptions, because all other will be caught by ELMAH anyway.
            if (context.ExceptionHandled)
                ErrorSignal.FromCurrentContext().Raise(context.Exception);
        }
    }
}