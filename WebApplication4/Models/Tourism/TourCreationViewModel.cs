using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebPortal.Models.CustomValidationAttributtes;
using WebPortal.Localizations;
using WebPortal.DbStuff.DataModels;

namespace WebPortal.Models.Tourism
{
    public class TourCreationViewModel
    {

        [OneOfTwoFieldsRequired("TourImgFile", "TourImgUrl")]
        [MaxFileSize(5)]
        [FileExtension(".jpg", "jpeg")]
        public IFormFile? TourImgFile { get; set; }

        [OneOfTwoFieldsRequired("TourImgFile", "TourImgUrl")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? TourImgUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(TourismLoc), ErrorMessageResourceName = nameof(TourismLoc.Validation_Name_Required))]
        [MaxLength(40)]
        [IsUniqTourName]
        public string TourName { get; set; }

        [Required(ErrorMessageResourceType = typeof(TourismLoc), ErrorMessageResourceName = nameof(TourismLoc.Validation_Author_Required))]
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(TourismLoc), ErrorMessageResourceName = nameof(TourismLoc.Validation_description_max_symbol))]
        public string? Description { get; set; }

        [Range(1, 50, ErrorMessageResourceType = typeof(TourismLoc), ErrorMessageResourceName = nameof(TourismLoc.Validation_Price))]
        [Required(ErrorMessageResourceType = typeof(TourismLoc), ErrorMessageResourceName = nameof(TourismLoc.Validation_Price_Required))]
        public double Price { get; set; }
        public List<SelectListItem> AllUsers { get; set; } = new List<SelectListItem>();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public List<ToursAutorStatiscticModel> ToursStatiscticModel { get; set; } = new List<ToursAutorStatiscticModel>();
    }
}
