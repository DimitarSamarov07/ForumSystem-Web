using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Categories
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        Task<IEnumerable<T>> GetAll<T>();

        IQueryable<T> GetAllAsQueryable<T>();

        Task CreateCategory(string title, string imageUrl, string description);

        Task RemoveCategory(int id);

        Task<T> GetByIdAsync<T>(int id);
    }
}
