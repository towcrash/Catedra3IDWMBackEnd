using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace catedra3.src.Validators
{
    public class ContainsNumberAttribute : ValidationAttribute
    {
        public ContainsNumberAttribute() : base("The field {0} must contain at least one number.")
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string stringValue = value.ToString();
            
            if (!stringValue.Any(char.IsDigit))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}