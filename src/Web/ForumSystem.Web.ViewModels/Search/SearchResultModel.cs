namespace ForumSystem.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using Posts;

    public class SearchResultModel
    {
        public IEnumerable<PostListingViewModel> Posts { get; set; }

        public string SearchQuery { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }

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

        public bool EmptySearchResults { get; set; }
    }
}
