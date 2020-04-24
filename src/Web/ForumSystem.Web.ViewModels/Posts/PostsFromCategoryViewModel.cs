namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using Categories;

    public class PostsFromCategoryViewModel
    {
        public CategoryListingViewModel Category { get; set; }

        public IEnumerable<PostAdminListingModel> Posts { get; set; }
    }
}
