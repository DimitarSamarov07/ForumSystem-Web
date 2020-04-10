namespace ForumSystem.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Common;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Services.Messaging;
    using ForumSystem.Web.ViewModels;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Home;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly IEmailSender sender;

        public HomeController(
                              ICategoryService categoryService,
                              IPostService postService,
                              IEmailSender sender)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.sender = sender;
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

        public IActionResult Privacy()
        {
            return this.RedirectToAction("Index", "Document", new { title = GlobalConstants.PrivacyPageDocumentTitle });
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}
