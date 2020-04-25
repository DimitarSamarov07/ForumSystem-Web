namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data.Models.Replies;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Services.Data.Replies;
    using ForumSystem.Web.ViewModels.Reply;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ReplyController : BaseController
    {
        private readonly IPostService postService;
        private readonly IReplyService replyService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReplyController(
                                IPostService postService,
                                IReplyService replyService,
                                UserManager<ApplicationUser> userManager)
        {
            this.postService = postService;
            this.replyService = replyService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Create(int id)
        {
            if (!await this.postService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var post = await this.postService.GetByIdAsync(id);

            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,

                AuthorId = user.Id,
                AuthorName = this.User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                KarmaPoints = user.KarmaPoints,
                IsAuthorAdmin = this.User.IsInRole("Admin"),

                CategoryId = post.Category.Id,
                CategoryName = post.Category.ImageUrl,
                CategoryImageUrl = post.Category.ImageUrl,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddReply(PostReplyModel model)
        {
            if (!await this.postService.DoesItExist(model.PostId))
            {
                return this.NotFound();
            }

            var user = await this.userManager.FindByNameAsync(this.User.Identity.Name);
            var reply = new Reply
            {
                PostId = model.PostId,
                Content = new HtmlSanitizer().Sanitize(model.ReplyContent),
                Author = user,
            };

            await this.replyService.CreateReplyAsync(reply);

            return this.RedirectToAction("Index", "Post", new { id = model.PostId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.replyService.DoesItExist(id))
            {
                return this.NotFound();
            }

            var model = await this.replyService.GetReplyById<EditReplyModel>(id);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditReplyModel model)
        {
            if (!await this.postService.DoesItExist(model.PostId))
            {
                return this.NotFound();
            }

            var serviceModel = new EditReplyServiceModel
            {
                ReplyId = model.ReplyId,
                Content = model.Content,
            };

            await this.replyService.EditReplyContent(serviceModel);
            return this.RedirectToAction("Index", "Post", new { id = model.PostId });
        }
    }
}
