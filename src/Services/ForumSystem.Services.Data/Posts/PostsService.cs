﻿namespace ForumSystem.Services.Data.Posts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.Infrastructure.Extensions;
    using ForumSystem.Web.ViewModels.Posts;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;
    using Models.Posts;

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
            return await this.GetAllFromCategoryАsQueryable<T>(categoryId)
                .ToListAsync();
        }

        public IQueryable<T> GetAllFromCategoryАsQueryable<T>(int categoryId)
            where T : class
        {
            var obj = this.postsRepository.All()
                 .Where(x => x.CategoryId == categoryId)
                 .Include(x => x.Author)
                 .To<T>();

            return obj;
        }

        public async Task<int> CreatePostAsync(PostCreateServiceModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = new HtmlSanitizer().Sanitize(model.Content),
                AuthorId = model.AuthorId,
                CategoryId = model.CategoryId,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
            return post.Id;
        }

        public async Task RemovePostAsync(int id)
        {
            var post = await this.GetByIdAsync(id);

            this.postsRepository.Delete(post);
            this.postsRepository.SaveChangesAsync().Wait();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            var obj = await this.postsRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();

            return obj;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var obj = await this.postsRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();

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

        public IQueryable<T> GetFilteredPosts<T>(string searchQuery)
        {
            // This is the fulltext variant but sadly when using EF there is no big difference, so I prefer the non-commented way
            // var posts = this.postsRepository.Contains("Title", searchQuery);
            // if (string.IsNullOrWhiteSpace(searchQuery))
            // {
            //     return posts.To<T>();
            // }
            // var search = searchQuery.ToLower();
            // var filteredPosts = posts.Where(
            //         p => p.Title
            //                  .ToLower()
            //                  .Contains(search)
            //              || p.Content
            //                  .ToLower()
            //                  .Contains(search))
            //     .To<T>();
            // return filteredPosts;

            var posts = this.postsRepository.All();
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }

            var search = searchQuery.ToLower();
            var filteredPosts = posts.Where(
                    p => p.Title
                             .ToLower()
                             .Contains(search)
                         || p.Content
                             .ToLower()
                             .Contains(search))
                .To<T>();
            return filteredPosts;
        }

        public async Task<IQueryable<T>> GetFilteredPosts<T>(int categoryId, string searchQuery)
        {
            var category = await this.categoryService.GetByIdAsync(categoryId);
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }

            searchQuery = searchQuery.ToLower();
            var filteredPosts = category.Posts
                .Where(
                    p => p.Title.ToLower().Contains(searchQuery)
                         || p.Content.ToLower().Contains(searchQuery));

            var result = filteredPosts.AsQueryable().To<T>();
            return result;
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

        public async Task EditPostContent(EditPostModel model)
        {
            var post = await this.GetByIdAsync(model.PostId);
            post.Content = new HtmlSanitizer().Sanitize(model.Content);
            this.postsRepository.Update(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task<bool> DoesItExist(int id)
        {
            var obj = await this.postsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            return obj != null;
        }
    }
}
