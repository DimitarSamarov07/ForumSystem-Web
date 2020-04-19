using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using CloudinaryDotNet;
    using Data.Models;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Query.Internal;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using ViewModels;
    using ViewModels.Categories;

    public class CategoryController : AdministrationController
    {
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

        public async Task<IActionResult> Index()
        {
            var categories = await this.categoryService.GetAllAsQueryable<CategoryListingViewModel>();

            var model = new CategoryIndexModel
            {
                CategoryList = categories,
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

            if (model.NewImage != null)
            {
                model.ImageUrl = await this.Upload(model.NewImage);
            }

            await this.categoryService.EditCategory(model);

            return this.RedirectToAction("Index", "Category");
        }

        [HttpPost]
        public async Task<ActionResult<DeleteCategoryModel>> Delete(int id)
        {
            await this.categoryService.RemoveCategory(id);
            return new DeleteCategoryModel { Id = id };
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
