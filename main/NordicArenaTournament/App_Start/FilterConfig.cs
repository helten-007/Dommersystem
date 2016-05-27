using System.Web;
using System.Web.Mvc;
using NordicArenaTournament.ErrorHandling;

namespace NordicArenaTournament
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new DbValidationExceptionFilter());
            filters.Add(new TraceWriterErrorFilter());
        }
    }
}