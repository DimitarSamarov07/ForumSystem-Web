namespace ForumSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private const int CategoriesToTake = 10;

        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly IEmailSender sender;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
                              ICategoryService categoryService,
                              IPostService postService,
                              IEmailSender sender,
                              UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.sender = sender;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.HomeIndexModel();
            return this.View(model);
        }

        public IActionResult Rules()
        {
            return this.RedirectToAction("Index", "Document", new { title = GlobalConstants.RulesPageDocumentTitle });
        }

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.RedirectToAction("Index", "Document", new { title = GlobalConstants.PrivacyPageDocumentTitle });
        }

        private async Task<IndexViewModel> HomeIndexModel()
        {
            var latestPosts = await this.postService.GetLatestPosts<PostListingViewModel>(10);
            var popularPosts = await this.postService.GetMostPopularPosts<PostListingViewModel>(10);
            var categories = await this.categoryService
                .GetAll<CategoryListingViewModel>();

            categories = categories.Take(CategoriesToTake);

            latestPosts = await this.CheckPostsForAuthorRole(latestPosts);
            popularPosts = await this.CheckPostsForAuthorRole(popularPosts);

            return new IndexViewModel
            {
                LatestPosts = latestPosts,
                PopularPosts = popularPosts,
                SearchQuery = string.Empty,
                Categories = categories,
            };
        }

        private async Task<IEnumerable<PostListingViewModel>> CheckPostsForAuthorRole(
            IEnumerable<PostListingViewModel> model)
        {
            foreach (var post in model)
            {
                var user = await this.userManager.FindByIdAsync(post.AuthorId);
                post.IsAuthorAdmin = await this.userManager.IsUserAdmin(user);
            }

            return model;
        }
    }
}
