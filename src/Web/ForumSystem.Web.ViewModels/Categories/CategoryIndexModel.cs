namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoryIndexModel
    {
        public IEnumerable<CategoryListingViewModel> CategoryList { get; set; }
    }
}
