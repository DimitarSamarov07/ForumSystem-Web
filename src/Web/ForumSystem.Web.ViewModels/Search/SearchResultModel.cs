namespace ForumSystem.Web.ViewModels.Search
{
    using System.Collections.Generic;
    using Posts;

    public class SearchResultModel
    {
        public IEnumerable<PostListingViewModel> Posts { get; set; }

        public string  SearchQuery { get; set; }

        public bool EmptySearchResults { get; set; }
    }
}
