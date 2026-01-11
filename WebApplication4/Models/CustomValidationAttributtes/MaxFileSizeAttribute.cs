using System.ComponentModel.DataAnnotations;
using WebPortal.Models.Tourism;

namespace WebPortal.Models.CustomValidationAttributtes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private int _maxFileSize; 

        public MaxFileSizeAttribute(int maxFileSizeMb)
        {
            _maxFileSize = maxFileSizeMb * 1024 * 1024;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null) 
            {
                return ValidationResult.Success;
            }
                
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"Maximum allowed file size is {_maxFileSize / (1024 * 1024)} MB");
            }
                

            return ValidationResult.Success;
        }
    }
}
