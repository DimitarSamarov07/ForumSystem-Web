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
    using Services.Data.Categories;
    using Services.Data.Posts;
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

        private async Task<string> Upload(IFormFile file)
        {
            var uri = await this.cloudinary.UploadAsync(file);
            return uri;
        }
    }
}
