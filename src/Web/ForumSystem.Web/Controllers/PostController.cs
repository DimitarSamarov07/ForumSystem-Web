namespace ForumSystem.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using Services.Mapping;
    using ViewModels.PostReplies;
    using ViewModels.Posts;

    public class PostController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;

        public PostController(
                              UserManager<ApplicationUser> userManager,
                              IPostService postService)
        {
            this.userManager = userManager;
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            var post = await this.postService.GetByIdAsync(id);
            var postModel = await this.postService.GetByIdAsync<PostIndexModel>(id);
            postModel.IsAuthorAdmin = this.IsUserAdmin(post.User);
            postModel.Replies = post.Replies.AsQueryable().To<PostReplyModel>();

            return this.View(postModel);
        }

        private bool IsUserAdmin(ApplicationUser user)
        {
            return this.userManager.GetRolesAsync(user).Result.Contains("Admin");
        }
    }
}
