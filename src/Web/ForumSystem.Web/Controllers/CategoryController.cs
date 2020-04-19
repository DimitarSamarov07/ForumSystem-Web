namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using ForumSystem.Common;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Web.Infrastructure.Attributes;
    using ForumSystem.Web.Infrastructure.Extensions;
    using ForumSystem.Web.Infrastructure.Helpers;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Auth(GlobalConstants.AdministratorRoleName)]
    public class CategoryController : BaseController
    {
        private const int PostsPerPageDefaultValue = 30;
        private const int CategoriesPerPageDefaultValue = 10;

        private readonly ICategoryService categoryService;
        private readonly IPostService postsService;
        private readonly Cloudinary cloudinary;
        private readonly UserManager<ApplicationUser> userManager;

        public CategoryController(
                                  ICategoryService categoryService,
                                  IPostService postsService,
                                  Cloudinary cloudinary,
                                  UserManager<ApplicationUser> userManager)
        {
            this.categoryService = categoryService;
            this.postsService = postsService;
            this.cloudinary = cloudinary;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1, int perPage = CategoriesPerPageDefaultValue)
        {
            var categories = await this.categoryService.GetAllAsQueryable<CategoryListingViewModel>();
            var pagesCount = (int)Math.Ceiling(categories.Count() / (decimal)perPage);

            if (page > pagesCount)
            {
                page = 1;
            }

            var model = new CategoryIndexModel()
            {
                PagesCount = pagesCount,
                CurrentPage = page,
                CategoryList = PaginationHelper
                    .CreateForPage(categories.OrderBy(f => f.NumberOfPosts), perPage, page),
            };

            return this.View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string searchQuery, int page = 1, int perPage = PostsPerPageDefaultValue)
        {
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            IQueryable<PostListingViewModel> posts;

            posts = !string.IsNullOrWhiteSpace(searchQuery) ?
                this.postsService.GetFilteredPosts<PostListingViewModel>(searchQuery)
                : this.postsService.GetAllFromCategoryАsQueryable<PostListingViewModel>(id);

            var pagesCount = (int)Math.Ceiling(posts.Count() / (decimal)perPage);

            posts = PaginationHelper.CreateForPage(posts, perPage, page);

            var category = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            foreach (var item in posts)
            {
                item.Category = category;
                item.IsAuthorAdmin = await this.userManager.IsUserAdmin(item.AuthorId);
            }

            var model = new CategoryDetailsViewModel
            {
                Category = category,
                Posts = posts,
                PagesCount = pagesCount,
                CurrentPage = page,
                SearchQuery = searchQuery,
            };

            return this.View(model);
        }

        //[Auth(GlobalConstants.AdministratorRoleName)]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    if (!await this.DoesItExist(id))
        //    {
        //        return this.NotFound();
        //    }

        //    var model = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

        //    return this.View(model);
        //}

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Search(int id, string searchQuery)
        {
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            return this.RedirectToAction("Details", new { id, searchQuery });
        }

        private async Task<bool> DoesItExist(int id)
        {
            var result = await this.categoryService.DoesItExist(id);

            return result;
        }
    }
}
