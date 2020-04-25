namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using ViewModels.Categories;
    using ViewModels.Posts;

    public class PostController : AdministrationController
    {
        private readonly IPostService postService;
        private readonly ICategoryService categoriesService;
        private readonly UserManager<ApplicationUser> userManager;

        public PostController(IPostService postService, ICategoryService categoriesService, UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public async Task<ViewResult> ByCategory(int id)
        {
            var posts = this.postService.GetAllFromCategoryАsQueryable<PostAdminListingModel>(id);

            var model = new PostsFromCategoryViewModel
            {
                Category = await this.categoriesService.GetByIdAsync<CategoryListingViewModel>(id),
                Posts = posts,
            };
            return this.View(model);
        }

        public async Task<IActionResult> Create(int id)
        {
            if (!await this.categoriesService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var category = await this.categoriesService.GetByIdAsync<CategoryListingViewModel>(id);

            var model = new NewPostModel
            {
                CategoryName = category.Title,
                CategoryId = category.Id,
                CategoryImageUrl = category.ImageUrl,
                AuthorName = this.User.Identity.Name,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            model.AuthorId = this.userManager.GetUserId(this.User);
            int postId = await this.postService.CreatePostAsync(model);

            return this.RedirectToAction("ByCategory", "Post", new { id = model.CategoryId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var model = await this.postService.GetByIdAsync<EditPostModel>(id);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditPostModel model)
        {
            if (!await this.postService.DoesItExist(model.PostId))
            {
                return this.NotFound();
            }

            await this.postService.EditPostContent(model);

            return this.RedirectToAction("ByCategory", "Post", new { id = model.CategoryId });
        }

        [HttpPost]
        public async Task<ActionResult<DeleteCategoryModel>> Delete(int id)
        {
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var post = await this.postService.GetByIdAsync(id);

            await this.postService.RemovePostAsync(id);
            return new DeleteCategoryModel { Id = id };
        }
    }
}
