using System.ComponentModel.DataAnnotations;
using WebPortal.Models.CustomValidationAttributtes;

namespace WebPortal.Models.Tourism
{
    public class NextArticleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Days for Title is required")]
        [MinMax(1,12)]
        public int? Days { get; set; }

        [Required(ErrorMessage = "The Title Name is required")]
        [RequiredWord("travel")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "The Image URL for Title is required")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string? URL { get; set; }
        public bool CanDelete { get; set; }
    }
}
