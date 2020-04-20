namespace ForumSystem.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Categories;
    using ForumSystem.Data;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels;
    using Web.ViewModels.Categories;
    using Xunit;

    public class CategoryTests
    {
        private CategoriesService service;
        private EfDeletableEntityRepository<Category> repository;
        private Category testCategory1;
        private Category testCategory2;

        public CategoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(options.Options);
            this.repository = new EfDeletableEntityRepository<Category>(dbContext);
            this.service = new CategoriesService(this.repository);
            this.testCategory1 = new Category
            {
                Title = "Test 1234",
                Description = "Test 1234",
                ImageUrl = "www.google.com/pesho.jpg",
            };

            this.testCategory2 = new Category
            {
                Title = "Hi test category 12345",
                Description = "Bye test category 12345",
                ImageUrl = "www.google.com/pesho1.jpg",
            };
        }

        [Fact]
        public async Task CreateMethodAddsTheCategory()
        {
            await this.service.CreateCategory(
                "Hello",
                "https://miro.medium.com/max/1200/1*mk1-6aYaf_Bes1E3Imhc0A.jpeg",
                "Hallo there");
            var count = this.repository.All().Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateMethodSetsAllProperties()
        {
            await this.service.CreateCategory(
                "Hello",
                "hi.jpg",
                "Hallo there");

            var obj = await this.repository.All().FirstOrDefaultAsync();

            Assert.Equal("Hello", obj.Title);
            Assert.Equal("hi.jpg", obj.ImageUrl);
            Assert.Equal("Hallo there", obj.Description);
        }

        [Fact]
        public async Task DeleteMethodWorks()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.AddAsync(this.testCategory2);
            await this.repository.SaveChangesAsync();
            await this.service.RemoveCategory(this.testCategory1.Id);

            var count = this.repository.All().Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetByIdWorks()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var resultObj = await this.service.GetByIdAsync(this.testCategory1.Id);

            Assert.Equal(this.testCategory1, resultObj);
        }

        [Fact]
        public async Task GetByIdGenericWorks()
        {
            AutoMapperConfig.RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"), typeof(Category).GetTypeInfo().Assembly);

            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var expected = new CategoryListingViewModel
            {
                Id = this.testCategory1.Id,
                Title = this.testCategory1.Title,
                Description = this.testCategory1.Description,
                ImageUrl = this.testCategory1.ImageUrl,
                NumberOfPosts = 0,
                HasRecentPost = true,
            };

            var result = await this.service.GetByIdAsync<CategoryListingViewModel>(this.testCategory1.Id);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task DoesItExistReturnsTrueIfItExists()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var obj = await this.service.DoesItExist(this.testCategory1.Id);

            Assert.True(obj);
        }

        [Fact]
        public async Task DoesItExistReturnsFalseIfItDoesNotExists()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var obj = await this.service.DoesItExist(this.testCategory2.Id);

            Assert.False(obj);
        }

        [Fact]
        public async Task EditCategoryChangesAllTheNeededProperties()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var model = new EditCategoryModel
            {
                CategoryId = this.testCategory1.Id,
                CategoryTitle = "Warframe",
                CategoryDescription = "A category for the popular game warframe created by DE",
                ImageUrl = "test123.png",
            };

            await this.service.EditCategory(model);

            Assert.Equal(model.CategoryTitle, this.testCategory1.Title);
            Assert.Equal(model.CategoryDescription, this.testCategory1.Description);
            Assert.Equal(model.ImageUrl, this.testCategory1.ImageUrl);
        }

        [Fact]
        public async Task EditCategoryIgnoresNullImageUrlAndKeepsTheOldOne()
        {
            await this.repository.AddAsync(this.testCategory1);
            await this.repository.SaveChangesAsync();

            var model = new EditCategoryModel
            {
                CategoryId = this.testCategory1.Id,
                CategoryTitle = "Warframe",
                CategoryDescription = "A category for the popular game warframe created by DE",
                ImageUrl = null,
            };

            await this.service.EditCategory(model);

            Assert.NotNull(this.testCategory1.ImageUrl);
        }
    }
}
