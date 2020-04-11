using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
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

        public SearchController(
            IPostService postsService,
            ICategoryService categoryService)
        {
            this.postsService = postsService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> Results(string searchQuery)
        {
            var listingModel = await this.postsService.GetFilteredPosts<PostListingViewModel>(searchQuery);
            var results = !string.IsNullOrEmpty(searchQuery) && !listingModel.Any();
            foreach (var item in listingModel)
            {
                item.Category = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(item.CategoryId);
            }

            var model = new SearchResultModel
            {
                Posts = listingModel,
                SearchQuery = searchQuery,
                EmptySearchResults = results,
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
