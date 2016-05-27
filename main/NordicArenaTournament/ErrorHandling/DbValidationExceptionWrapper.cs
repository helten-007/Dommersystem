using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;

namespace NordicArenaTournament.ErrorHandling
{
    public class DbValidationExceptionWrapper
    {
        public static Exception Wrap(DbEntityValidationException exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DbEntityValidationException: ");
            foreach (var res in exception.EntityValidationErrors)
            {
                sb.AppendLine("  " + res.Entry.Entity);
                foreach (var er in res.ValidationErrors)
                {
                    sb.AppendLine(String.Format("    {0}: {1}", er.PropertyName, er.ErrorMessage));
                }
            }
            return new DetailedDbValidationException(sb.ToString(), exception);
        }        
    }

    /// <summary>
    /// A DbValidationException where you don't actually have to run in debug mode to get the validation exception details
    /// </summary>
    public class DetailedDbValidationException : Exception 
    {
        public DetailedDbValidationException(String message, DbEntityValidationException innerException) :base(message, innerException) {

        }
    }
}