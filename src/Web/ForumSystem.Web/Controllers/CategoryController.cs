namespace ForumSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Web.ViewModels.Categories;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CategoryController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postsService;

        public CategoryController(ICategoryService categoryService, IPostService postsService)
        {
            this.categoryService = categoryService;
            this.postsService = postsService;
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
            var category = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            var posts =
                await this.postsService.GetAllFromCategory<PostListingViewModel>(id);

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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new AddCategoryModel();

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryModel model)
        {
            var imageUri = "images/category/category.png";

            await this.categoryService.CreateCategory(model.Title, imageUri, model.Description);

            return this.RedirectToAction("Index", "Category");
            // Controller name added just to make the code easier to understand
            // and not to be confused with the Home controller
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

            return this.View(model);
        }


        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await this.categoryService.RemoveCategory(id);
            return this.RedirectToAction("Index");
        }
    }
}
