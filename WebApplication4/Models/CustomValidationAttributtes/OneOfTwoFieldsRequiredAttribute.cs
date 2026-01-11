using System.ComponentModel.DataAnnotations;
using WebPortal.Models.Tourism;

namespace WebPortal.Models.CustomValidationAttributtes
{
    public class OneOfTwoFieldsRequiredAttribute : ValidationAttribute
    {
        public string _field1;
        public string _field2;

        public OneOfTwoFieldsRequiredAttribute(string field1, string field2)
        {
            _field1 = field1;
            _field2 = field2;
        }
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var viewModel = validationContext.ObjectInstance.GetType();

            var field1Value = viewModel.GetProperty(_field1)?.GetValue(validationContext.ObjectInstance);
            var field2Value = viewModel.GetProperty(_field2)?.GetValue(validationContext.ObjectInstance);

            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (field1Value == null && field2Value == null)
            {
                return new ValidationResult($"Please fill one of two field required field: {_field1} or {_field2}");
            }

            if (field1Value != null && field2Value != null)
            {
                return new ValidationResult("Please note that only one these two field should be entered");
            }

            return ValidationResult.Success;
        }
    }
}
