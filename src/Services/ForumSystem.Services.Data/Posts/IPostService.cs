using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Posts
{
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.Posts;
    using Models.Posts;

    public interface IPostService
    {
        Task<IEnumerable<T>> GetAllFromCategory<T>(int categoryId)
            where T : class;

        IQueryable<T> GetAllFromCategoryАsQueryable<T>(int categoryId)
            where T : class;

        Task<int> CreatePostAsync(PostCreateServiceModel model);

        Task RemovePostAsync(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task<Post> GetByIdAsync(int id);

        Task<IEnumerable<T>> GetLatestPosts<T>(int n);

        Task<IEnumerable<T>> GetMostPopularPosts<T>(int n);

        IQueryable<T> GetFilteredPosts<T>(string searchQuery);

        Task<IQueryable<T>> GetFilteredPosts<T>(int categoryId, string searchQuery);

        Task EditPostContent(EditPostModel model);

        Task<bool> DoesItExist(int id);
    }
}
