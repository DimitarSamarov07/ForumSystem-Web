using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Categories
{
    using System.Threading.Tasks;
    using ForumSystem.Data.Models;

    public interface ICategoryService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task CreateCategory(string title, string imageUrl, string description);

        Task RemoveCategory(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task<Category> GetByIdAsync(int id);
    }
}
