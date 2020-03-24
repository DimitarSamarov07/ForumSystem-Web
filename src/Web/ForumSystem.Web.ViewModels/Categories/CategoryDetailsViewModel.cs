namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using Posts;

    public class CategoryDetailsViewModel
    {
        public CategoryListingViewModel Category { get; set; }

        public IEnumerable<PostListingViewModel> Posts { get; set; }

        public string SearchQuery { get; set; }
    }
}
