namespace ForumSystem.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Nest;
    using Web.Infrastructure.Extensions;

    public class CategoriesService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(
             IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.categoriesRepository.All().IncludeAll().To<T>().ToListAsync();
        }

        public IQueryable<T> GetAllAsQueryable<T>()
        {
            return this.categoriesRepository.All().To<T>();
        }

        public async Task CreateCategory(string title, string imageUrl, string description)
        {
            var category = new Category
            {
                Title = title,
                ImageUrl = imageUrl,
                Description = description,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task RemoveCategory(int id)
        {
            var category = await this.GetByIdAsync(id);
            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var obj = await this.categoriesRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefaultAsync();

            return obj;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            var category = await this.categoriesRepository.All().Where(x => x.Id == id).FirstOrDefaultAsync();

            return category;
        }
    }
}
