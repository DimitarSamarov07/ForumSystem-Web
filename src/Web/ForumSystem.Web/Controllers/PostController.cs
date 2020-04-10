namespace ForumSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Ganss.XSS;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using Services.Mapping;
    using ViewModels.Categories;
    using ViewModels.Posts;
    using ViewModels.Reply;

    public class PostController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly ICategoryService categoryService;
        private readonly IHtmlSanitizer sanitizer;

        public PostController(
                              UserManager<ApplicationUser> userManager,
                              IPostService postService,
                              ICategoryService categoryService,
                              IHtmlSanitizer sanitizer)
        {
            this.userManager = userManager;
            this.postService = postService;
            this.categoryService = categoryService;
            this.sanitizer = sanitizer;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var post = await this.postService.GetByIdAsync(id);
            var postModel = await this.postService.GetByIdAsync<PostIndexModel>(id);
            postModel.IsAuthorAdmin = this.IsUserAdmin(post.User);
            postModel.PostContent = new HtmlSanitizer().Sanitize(postModel.PostContent);

            return this.View(postModel);
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var category = await this.categoryService.GetByIdAsync<CategoryListingViewModel>(id);

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
        [Authorize]
        public async Task<IActionResult> AddPost(NewPostModel model)
        {
            model.UserId = this.userManager.GetUserId(this.User);
            int postId = await this.postService.CreatePostAsync(model);

            return this.RedirectToAction("Index", "Post", new { id = postId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var model = await this.postService.GetByIdAsync<PostListingViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await this.postService.GetByIdAsync(id);

            await this.postService.RemovePostAsync(id);
            return this.RedirectToAction("Details", "Category", new { id = post.CategoryId});
        }
        public Task<IActionResult> Edit()
        {
            return null;
        }

        private bool IsUserAdmin(ApplicationUser user)
        {
            return this.userManager.GetRolesAsync(user).Result.Contains("Admin");
        }

        
    }
}
