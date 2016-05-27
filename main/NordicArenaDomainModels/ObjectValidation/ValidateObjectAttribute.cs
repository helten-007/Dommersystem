using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NordicArenaDomainModels.ObjectValidation
{
    // Thanks: http://www.technofattie.com/2011/10/recursive-validation-using.html
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var results = new List<ValidationResult>(); 
            var context = new ValidationContext(value, null, null);

            if (value is IEnumerable) {
                foreach (object elem in (IEnumerable)value)
                {
                    var elemContext = new ValidationContext(elem, null, null);
                    Validator.TryValidateObject(elem, elemContext, results, true);
                }
            }
            else 
            {
                Validator.TryValidateObject(value, context, results, true);
            }                

            if (results.Count != 0)
            {
                var compositeResults = new CompositeValidationResult(String.Format("Validation for {0} failed!", validationContext.DisplayName));
                results.ForEach(compositeResults.AddResult);

                return compositeResults;
            }

            return ValidationResult.Success;
        }
    }

    public class CompositeValidationResult : ValidationResult
    {
        private readonly List<ValidationResult> _results = new List<ValidationResult>();

        public IEnumerable<ValidationResult> Results
        {
            get
            {
                return _results;
            }
        }

        public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
        public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
        protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

        public void AddResult(ValidationResult validationResult)
        {
            _results.Add(validationResult);
        }
    }   
}