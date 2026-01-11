using System.ComponentModel.DataAnnotations;
using WebPortal.Models.Tourism;

namespace WebPortal.Models.CustomValidationAttributtes
{

    public class FileExtensionAttribute : ValidationAttribute
    {
        private string[] _extensions;

        public FileExtensionAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
            {
                return ValidationResult.Success;
            }
               
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_extensions.Contains(extension))
            {
                return new ValidationResult($"Allowed file formats: {string.Join(", ", _extensions)}");
            }


            return ValidationResult.Success;
        }
    }
}
