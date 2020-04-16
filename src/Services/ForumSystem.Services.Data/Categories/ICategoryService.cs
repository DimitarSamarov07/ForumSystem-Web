using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Categories
{
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Models;
    using Web.ViewModels.Categories;

    public interface ICategoryService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<IQueryable<T>> GetAllAsQueryable<T>();

        Task CreateCategory(string title, string imageUrl, string description);

        Task RemoveCategory(int id);

        Task<T> GetByIdAsync<T>(int id);

        Task<Category> GetByIdAsync(int id);

        Task<bool> DoesItExist(int id);

        Task EditCategory(EditCategoryModel model);

    }
}
