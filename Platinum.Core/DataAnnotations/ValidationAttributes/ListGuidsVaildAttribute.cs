using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Platinum.Core.DataAnnotations.ValidationAttributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class ListGuidsVaildAttribute : ValidationAttribute
    {
        protected readonly List<ValidationResult> validationResults = new List<ValidationResult>();
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public ListGuidsVaildAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (!(value is IEnumerable list)) return false;

            var isValid = true;

            foreach (var item in list)
            {
                var validationContext = new ValidationContext(item);
                var isItemValid = Validator.TryValidateObject(item, validationContext, validationResults, true);
                isValid &= isItemValid;
            }
            return isValid;
        }
    }
}
