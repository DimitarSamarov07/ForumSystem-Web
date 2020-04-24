namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Infrastructure.Attributes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Categories;
    using Services.Data.Posts;
    using ViewModels.Categories;
    using ViewModels.Posts;

    public class PostController : AdministrationController
    {
        private readonly IPostService postService;
        private readonly ICategoryService categoriesService;

        public PostController(IPostService postService, ICategoryService categoriesService)
        {
            this.postService = postService;
            this.categoriesService = categoriesService;
        }

        public async Task<ViewResult> ByCategory(int id)
        {
            var posts = await this.postService.GetAllFromCategory<PostAdminListingModel>(id);

            var model = new PostsFromCategoryViewModel
            {
                Category = await this.categoriesService.GetByIdAsync<CategoryListingViewModel>(id),
                Posts = posts,
            };
            return this.View(model);
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

            return this.RedirectToAction("ByCategory", "Post", new { id = model.CategoryId });
        }

        [HttpPost]
        [Auth(GlobalConstants.AdministratorRoleName)]
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
