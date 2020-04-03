namespace ForumSystem.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.Infrastructure.Extensions;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Nest;
    using Web.ViewModels.Posts;

    public class PostsService : IPostService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;
        private readonly IElasticClient elasticClient;

        public PostsService(
                            IDeletableEntityRepository<Post> postsRepository,
                            IElasticClient elasticClient)
        {
            this.postsRepository = postsRepository;
            this.elasticClient = elasticClient;
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

            await this.elasticClient.IndexDocumentAsync(post);
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
