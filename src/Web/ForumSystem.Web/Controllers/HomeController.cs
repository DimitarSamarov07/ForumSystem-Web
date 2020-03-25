namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.Models;
    using ForumSystem.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using Services.Mapping;
    using ViewModels.Categories;
    using ViewModels.Home;
    using ViewModels.Posts;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;

        public HomeController(
                              ICategoryService categoryService,
                              IPostService postService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.HomeIndexModel();
            return this.View(model);
        }

        private async Task<IndexViewModel> HomeIndexModel()
        {
            var latestPosts = await this.postService.GetLatestPosts<PostListingViewModel>(10);
            var popularPosts = await this.postService.GetMostPopularPosts<PostListingViewModel>(10);
            var categories = await this.categoryService
                .GetAll<CategoryListingViewModel>();

            return new IndexViewModel()
            {
                LatestPosts = latestPosts,
                PopularPosts = popularPosts,
                SearchQuery = string.Empty,
                Categories = categories,

            };
        }

        private CategoryListingViewModel GetCategoryListingForPost(Post p)
        {
            var category = p.Category;
            return new CategoryListingViewModel()
            {
                Title = category.Title,
                ImageUrl = category.ImageUrl,
                Id = category.Id,
            };
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
