using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DailyScrum.Validations
{
    public class FirstNameValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = (string)value;

            if(!Regex.Match(name, "^[A-Z][a-zA-Z]*$").Success)
            {
                return new ValidationResult("Nie poprawne imię.");
            }

            return ValidationResult.Success;
        }
    }
}
