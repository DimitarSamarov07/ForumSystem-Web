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

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AddCategoryModel();

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryModel model)
        {
            var secureImageUri = await this.Upload(model.ImageUpload);

            await this.categoryService.CreateCategory(model.Title, secureImageUri, model.Description);

            return this.RedirectToAction("Index", "Category");

            // Controller name added just to make the code easier to understand
            // and not to be confused with the Home controller
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.categoryService.GetByIdAsync<EditCategoryModel>(id);
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCategoryModel model)
        {
            if (!await this.DoesItExist(model.CategoryId))
            {
                return this.NotFound();
            }

            await this.categoryService.EditCategory(model);

            return this.RedirectToAction("Index", "Category", new { id = model.CategoryId });
        }

        [Auth(GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            var model = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            await this.categoryService.RemoveCategory(id);
            return this.RedirectToAction("Index");
        }

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

        private async Task<string> Upload(IFormFile file)
        {
            var uri = await this.cloudinary.UploadAsync(file);
            return uri;
        }

        private async Task<bool> DoesItExist(int id)
        {
            var result = await this.categoryService.DoesItExist(id);

            return result;
        }
    }
}
