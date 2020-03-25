using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Posts
{
    using System.Threading.Tasks;
    using ForumSystem.Data.Models;

    public interface IPostService
    {
        Task<IEnumerable<T>> GetAllFromCategory<T>(int categoryId)
            where T : class;

        Task CreatePost(string title, string imageUrl, string content);

        Task RemovePost(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task<IEnumerable<T>> GetLatestPosts<T>(int n);

        Task<IEnumerable<T>> GetMostPopularPosts<T>(int n);
    }
}
