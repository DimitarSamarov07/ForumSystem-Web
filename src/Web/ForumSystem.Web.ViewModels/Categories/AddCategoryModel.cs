using Microsoft.AspNetCore.Http;

namespace ForumSystem.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryModel
    {
        [MinLength(6)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
