using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NordicArenaTournament.ErrorHandling
{
    public class TraceWriterErrorFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            Trace.TraceError("Logging exception from TraceWriterErrorFilter:");
            Trace.TraceError(filterContext.Exception.GetAsString());
        }
    }
}