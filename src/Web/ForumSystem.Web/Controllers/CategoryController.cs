namespace ForumSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Common;
    using Data.Models;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Posts;
    using Ganss.XSS;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postsService;
        private readonly Cloudinary cloudinary;

        public CategoryController(
                                  ICategoryService categoryService,
                                  IPostService postsService,
                                  Cloudinary cloudinary)
        {
            this.categoryService = categoryService;
            this.postsService = postsService;
            this.cloudinary = cloudinary;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAll<CategoryListingViewModel>();

            var model = new CategoryIndexModel()
            {
                CategoryList = categories.OrderBy(f => f.Title),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(int id, string searchQuery)
        {
            if (!await this.DoesItExist(id))
            {
                return this.NotFound();
            }

            var category = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            IEnumerable<PostListingViewModel> posts = new List<PostListingViewModel>();

            if (searchQuery != null)
            {
                posts =
                   await this.postsService.GetFilteredPosts<PostListingViewModel>(searchQuery);
            }

            else
            {
                posts =
                    await this.postsService.GetAllFromCategory<PostListingViewModel>(id);
            }

            foreach (var item in posts)
            {
                item.Category = category;
            }

            var model = new CategoryDetailsViewModel
            {
                Category = category,
                Posts = posts,
                SearchQuery = null,
            };

            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            var model = new AddCategoryModel();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> AddCategory(AddCategoryModel model)
        {
            var imageUri = await this.Upload(model.ImageUpload);

            await this.categoryService.CreateCategory(model.Title, imageUri, model.Description);

            return this.RedirectToAction("Index", "Category");

            // Controller name added just to make the code easier to understand
            // and not to be confused with the Home controller
        }

        [HttpGet]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
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
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Edit(EditCategoryModel model)
        {
            if (!await this.DoesItExist(model.CategoryId))
            {
                return this.NotFound();
            }

            await this.categoryService.EditCategory(model);

            return this.RedirectToAction("Index", "Category", new { id = model.CategoryId });
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.categoryService.RemoveCategory(id);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
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
