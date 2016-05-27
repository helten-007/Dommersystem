using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NordicArenaTournament.ErrorHandling
{
    public static class ExceptionExtensions
    {
        public static string GetAsString(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            AppendDefaultExceptionString(ex, sb);
            return sb.ToString();
        }

        private static void AppendDefaultExceptionString(this Exception ex, StringBuilder sb)
        {
            sb.AppendLine(ex.GetType().ToString());
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine();
            if (ex.InnerException != null)
            {
                sb.AppendLine("---Inner exception: ");
                AppendDefaultExceptionString(ex.InnerException, sb);
            }
        }
    }
}