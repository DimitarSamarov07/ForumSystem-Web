using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using Infrastructure.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Search;

    public class SearchController : BaseController
    {
        private readonly IPostService postsService;
        private readonly ICategoryService categoryService;
        private const int PostsPerPageDefaultValue = 10;

        public SearchController(
            IPostService postsService,
            ICategoryService categoryService)
        {
            this.postsService = postsService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Results(string searchQuery, int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            var listingModel = this.postsService.GetFilteredPosts<PostListingViewModel>(searchQuery);
            var pagesCount = (int)Math.Ceiling(listingModel.Count() / (decimal)perPage);

            listingModel = PaginationHelper.CreateForPage(listingModel, perPage, page);

            var results = !string.IsNullOrEmpty(searchQuery) && !listingModel.Any();

            var model = new SearchResultModel
            {
                Posts = listingModel,
                SearchQuery = searchQuery,
                EmptySearchResults = results,
                CurrentPage = page,
                PagesCount = pagesCount,
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Search(string searchQuery)
        {
            return this.RedirectToAction("Results", new { searchQuery });
        }
    }
}
