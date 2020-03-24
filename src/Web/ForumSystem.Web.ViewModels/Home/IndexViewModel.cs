using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Home
{
    using Categories;
    using Data.Models;
    using Posts;

    public class IndexViewModel
    {
        public string SearchQuery { get; set; }

        public IEnumerable<PostListingViewModel> LatestPosts { get; set; }

        public IEnumerable<PostListingViewModel> PopularPosts { get; set; }

        public IEnumerable<CategoryListingViewModel> Categories { get; set; }

    }
}
