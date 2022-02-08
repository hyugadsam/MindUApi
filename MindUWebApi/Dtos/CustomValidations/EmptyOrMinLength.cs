using System.ComponentModel.DataAnnotations;

namespace Dtos.CustomValidations
{
    public class EmptyOrMinLength : ValidationAttribute
    {
        private readonly int length;

        public EmptyOrMinLength(int Length)
        {
            length = Length;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if (value.ToString().Length < length)
            {
                return new ValidationResult($"If the password is supplied, the min length is {length}");
            }

            return ValidationResult.Success;
        }
    }
}
