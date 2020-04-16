namespace ForumSystem.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class CategoryIndexModel
    {
        public IEnumerable<CategoryListingViewModel> CategoryList { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int NextPage
        {
            get
            {
                if (this.CurrentPage >= this.PagesCount)
                {
                    return 1;
                }

                return this.CurrentPage + 1;
            }
        }

        public int PreviousPage
        {
            get
            {
                if (this.CurrentPage <= 1)
                {
                    return this.PagesCount;
                }

                return this.CurrentPage - 1;
            }
        }
    }
}
