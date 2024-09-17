using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class NotAllowedAttributes:ValidationAttribute
    {
        private char [] notallowed;
        public NotAllowedAttributes(char [] notallowed)
        {
            this.notallowed = notallowed;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            foreach (var item in notallowed)
            {
                if (value.ToString().Contains(item))
                {
                    return new ValidationResult($"{item} not allowed..");
                }
            }
            return ValidationResult.Success;
        }

    }
}
