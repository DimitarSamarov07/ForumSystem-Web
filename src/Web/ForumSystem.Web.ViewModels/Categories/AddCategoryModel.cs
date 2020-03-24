using Microsoft.AspNetCore.Http;

namespace ForumSystem.Web.ViewModels.Categories
{
    public class AddCategoryModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
