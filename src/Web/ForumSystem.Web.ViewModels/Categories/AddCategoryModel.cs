using Microsoft.AspNetCore.Http;

namespace ForumSystem.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure.Attributes;

    public class AddCategoryModel
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Length cannot be shorter than 6 characters!")]
        [MaxLength(40, ErrorMessage = "Length cannot be longer than 40 characters!")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(50, ErrorMessage = "Length cannot be shorter than 50 characters!")]
        [MaxLength(300, ErrorMessage = "Length cannot be longer than 300 characters!")]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public IFormFile ImageUpload { get; set; }
    }
}
