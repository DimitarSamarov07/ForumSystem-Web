namespace ForumSystem.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Categories;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Extensions;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Posts;

    public class PostsService : IPostService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly ICategoryService categoryService;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository,
            ICategoryService categoryService)
        {
            this.postsRepository = postsRepository;
            this.categoryService = categoryService;
        }

        public async Task<IEnumerable<T>> GetAllFromCategory<T>(int categoryId)
        where T : class
        {
            return await this.postsRepository.All()
                .Where(x => x.Category.Id == categoryId)
                .To<T>()
                .IncludeAll()
                .ToListAsync();
        }

        public async Task<int> CreatePostAsync(NewPostModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                ImageUrl = model.CategoryImageUrl,
                Content = model.Content,
                UserId = model.UserId,
                CategoryId = model.CategoryId,
            };

            await this.postsRepository.AddAsync(post);
            this.postsRepository.SaveChangesAsync().Wait();
            return post.Id;
        }

        public async Task RemovePostAsync(int id)
        {
            var post = await this.GetByIdAsync(id);

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var obj = await this.postsRepository.All().IncludeAll().Where(x => x.Id == id).FirstOrDefaultAsync();

            return obj;
        }

        public async Task<IEnumerable<T>> GetLatestPosts<T>(int n)
        {
            return await this.postsRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>()
                .Take(n)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetFilteredPosts<T>(string searchQuery)
        {
            var search = searchQuery.ToLower();
            var posts = this.postsRepository.All();
            var filteredPosts = await posts.Where(
                p => p.Title
                         .ToLower()
                         .Contains(search)
                     || p.Content
                         .ToLower()
                         .Contains(search))
                .To<T>()
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<T>> GetFilteredPosts<T>(int categoryId, string searchQuery)
        {
            var search = searchQuery;
            var category = await this.categoryService.GetByIdAsync(categoryId);

            var filteredPosts = await category.Posts.Where(
                p => p.Title.ToLower().Contains(search)
                     || p.Content.Contains(search))
                .AsQueryable()
                .To<T>()
                .ToListAsync();

            return filteredPosts;
        }

        public async Task<IEnumerable<T>> GetMostPopularPosts<T>(int n)
        {
            return await this.postsRepository
                .All()
                .OrderByDescending(p => p.Replies.Count)
                .Take(n)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var obj = await this.postsRepository.All().Where(x => x.Id == id).IncludeAll().To<T>().FirstOrDefaultAsync();

            return obj;
        }
    }
}
