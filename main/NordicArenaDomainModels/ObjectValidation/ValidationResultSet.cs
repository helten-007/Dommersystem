using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NordicArenaDomainModels.ObjectValidation
{
    /// <summary>
    /// Wrapping a result bool and its reason
    /// </summary>
    public class ValidationResultSet
    {
        public bool IsValid { get; set; }
        public HashSet<ValidationResult> Results { get; set; }
        public ValidationResultSet()
        {
            Results = new HashSet<ValidationResult>();
        }

        public String GetFirstErrorMessage()
        {
            if (IsValid) return String.Empty;
            return Results.FirstOrDefault().ToString();
        }
    }
}