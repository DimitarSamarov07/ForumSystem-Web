﻿namespace ForumSystem.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using Ganss.XSS;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using MoreLinq;
    using Nest;
    using Web.Infrastructure.Extensions;
    using Web.ViewModels.Categories;

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
            var obj = await this.categoriesRepository.All().IncludeAll().ToListAsync();
            foreach (var category in obj)
            {
                category.NumberOfUsers = await this.GetCountOfUsersInCategory(category.Id);
            }

            var objToReturn = obj.AsQueryable().To<T>();
            return objToReturn;
        }

        public async Task<IQueryable<T>> GetAllAsQueryable<T>()
        {
            var all = await this.GetAll<T>();
            var objToReturn = all.AsQueryable();
            return objToReturn;
        }

        public async Task CreateCategory(string title, string imageUrl, string description)
        {
            var category = new Category
            {
                Title = title,
                ImageUrl = imageUrl,
                Description = new HtmlSanitizer().Sanitize(description),
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

        public async Task<bool> DoesItExist(int id)
        {
            var obj = await this.categoriesRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            return obj != null;
        }

        public async Task EditCategory(EditCategoryModel model)
        {
            var category = await this.GetByIdAsync(model.CategoryId);
            category.Title = model.CategoryTitle;
            category.Description = new HtmlSanitizer().Sanitize(model.CategoryDescription);
            if (model.ImageUrl != null)
            {
                category.ImageUrl = model.ImageUrl;
            }

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();
        }

        private async Task<int> GetCountOfUsersInCategory(int id)
        {
            var category = await this.categoriesRepository.All()
                .Include(x => x.Posts)
                .FirstOrDefaultAsync(x => x.Id == id);

            return category.Posts
                .Where(x => !x.IsDeleted)
                .DistinctBy(x => x.AuthorId)
                .Count();

            // I had to do this because of an issue with EF. It will throw error if I try to make some kind of
            // Linq query through Automapper : https://github.com/dotnet/efcore/issues/18179#issuecomment-578862522
        }
    }
}
