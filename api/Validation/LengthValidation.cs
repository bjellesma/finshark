using System.ComponentModel.DataAnnotations;

namespace api.Validation
{
    public class LengthValidation : ValidationAttribute
    {
        public int MinLength { get; }
        public int MaxLength { get; }

        public LengthValidation(int minLength, int maxLength)
        {
            MinLength = minLength;
            MaxLength = maxLength;
            ErrorMessage = $"The field must be between {MinLength} and {MaxLength} characters.";
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                if (stringValue.Length < MinLength || stringValue.Length > MaxLength)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}