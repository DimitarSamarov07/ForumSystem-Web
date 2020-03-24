namespace ForumSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postsService;
        private readonly IMapper mapper;

        public CategoryController(ICategoryService categoryService, IPostService postsService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.postsService = postsService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return null;
        }

        public async Task<IActionResult> Details(int id, string searchQuery)
        {
            var category = await this.categoryService.GetByIdAsync<Category>(id);

            var posts =
                await this.postsService.GetAllFromCategory<PostListingViewModel>(id);

            var categoryListingModel = this.mapper.Map<Category, CategoryListingViewModel>(category);

            foreach (var item in posts)
            {
                item.Category = categoryListingModel;
            }

            var model = new CategoryDetailsViewModel
            {
                Category = categoryListingModel,
                Posts = posts,
                SearchQuery = null,
            };

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(AddCategoryModel model)
        {
            var imageUri = "images/category/category.png";

            await this.categoryService.CreateCategory(model.Title, imageUri, model.Description);

            return this.RedirectToAction("Index", "Category");
            // Controller name added just to make the code easier to understand
            // and not to be confused with the Home controller
        }
    }
}
