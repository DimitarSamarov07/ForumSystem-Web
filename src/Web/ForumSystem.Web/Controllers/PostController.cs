namespace ForumSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Common;
    using Data.Models;
    using Ganss.XSS;
    using Infrastructure.Attributes;
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
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var post = await this.postService.GetByIdAsync(id);
            var postModel = await this.postService.GetByIdAsync<PostIndexModel>(id);

            postModel.Replies = await this.CheckIfAuthorsAreAdmins(postModel.Replies);

            var user = await this.userManager.GetUserAsync(this.User);
            postModel.IsCurrentUserAuthorOrAdmin = await this.IsUserAuthorOrAdmin(user, post);

            postModel.IsAuthorAdmin = await this.IsUserAdmin(post.Author);

            return this.View(postModel);
        }

        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            if (!await this.categoryService.DoesItExist(id))
            {
                return this.NotFound();
            }

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
            model.AuthorId = this.userManager.GetUserId(this.User);
            int postId = await this.postService.CreatePostAsync(model);

            return this.RedirectToAction("Index", "Post", new { id = postId });
        }

        [Auth(GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var user = await this.userManager.GetUserAsync(this.User);

            var model = await this.postService.GetByIdAsync<PostListingViewModel>(id);

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Auth(GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var post = await this.postService.GetByIdAsync(id);

            await this.postService.RemovePostAsync(id);
            return this.RedirectToAction("Details", "Category", new { id = post.CategoryId });
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(EditPostModel model)
        {
            if (!await this.postService.DoesItExist(model.PostId))
            {
                return this.NotFound();
            }

            await this.postService.EditPostContent(model);

            return this.RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        private async Task<bool> IsUserAdmin(ApplicationUser user)
        {
            return await this.userManager.IsUserAdmin(user);
        }

        private async Task<bool> IsUserAuthorOrAdmin(ApplicationUser user, Post post)
        {
            if (user == null || post == null)
            {
                return false;
            }

            var bool1 = await this.userManager.IsUserAdmin(user);
            var bool2 = post.AuthorId == user.Id;

            return bool1 || bool2;
        }

        private async Task<IEnumerable<PostReplyModel>> CheckIfAuthorsAreAdmins(IEnumerable<PostReplyModel> postModelReplies)
        {
            foreach (var reply in postModelReplies)
            {
                var author = await this.userManager.FindByIdAsync(reply.AuthorId);
                reply.IsAuthorAdmin = await this.IsUserAdmin(author);
            }

            return postModelReplies;
        }
    }
}
