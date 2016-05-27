using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.ErrorHandling
{
    public class DbValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exc = filterContext.Exception;
            if (exc is DbEntityValidationException)
            {
                // Need to write a HTTP Handler in order to hook into event BEFORE elmah logs,
                // so I'm taking the easy way out for now by double-logging
                var detailedExc = DbValidationExceptionWrapper.Wrap((DbEntityValidationException)exc);
                Elmah.ErrorLog.Default.Log(new Elmah.Error(detailedExc)); 
            }
        }
    }
}