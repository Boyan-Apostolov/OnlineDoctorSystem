using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineDoctorSystem.Web.Infrastructure
{
    public class StartEndTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;

            if (DateTime.UtcNow.AddDays(-1).CompareTo(value) <= 0 && DateTime.UtcNow.AddYears(1).CompareTo(value) >= 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Грешно време!");
        }
    }
}
