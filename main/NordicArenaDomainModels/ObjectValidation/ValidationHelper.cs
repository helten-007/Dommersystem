using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using NordicArenaDomainModels.Lang;

namespace NordicArenaDomainModels.ObjectValidation
{
    /// <summary>
    /// Simplifies annotation-based validating and writing validation results
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// Validates an object and returns and outputs results to debug
        /// </summary>
        public static ValidationResultSet Validate(Object o) {
            var context = new ValidationContext(o);
            var results = new HashSet<ValidationResult>();
            var result = new ValidationResultSet();
            result.IsValid = Validator.TryValidateObject(o, context, result.Results, true);
            Debug.Write(LangUtilities.TextWriterToString((s) => WriteResult(s, result.Results)));           
            return result;           
        }

        /// <summary>
        /// Writes all results recursively to the writer
        /// </summary>
        private static void WriteResult(TextWriter sw, IEnumerable<ValidationResult> results, int indentLevel = 0)
        {
            foreach (var validationResult in results)
            {
                String line = String.Format("{0,"+ indentLevel + "}", String.Empty) + validationResult.ErrorMessage.PadLeft(indentLevel, ' ');
                sw.WriteLine(line);
                if (validationResult is CompositeValidationResult)
                {
                    WriteResult(sw, ((CompositeValidationResult)validationResult).Results, indentLevel + 2);
                }
            }
        }

        public static void WriteResultToDebug(IEnumerable<ValidationResult> results)
        {
            Debug.Write(LangUtilities.TextWriterToString((s) => WriteResult(s, results)));   
        }
    }
}