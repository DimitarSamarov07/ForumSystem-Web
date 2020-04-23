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
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Web.ViewModels;
    using Web.ViewModels.Categories;
    using Xunit;

    public class CategoryTests
    {
        private CategoriesService service;
        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;
        private Category testCategory1;
        private Category testCategory2;

        public CategoryTests()
        {
            this.InitializeMapper();
            this.InitializeDatabaseAndRepositories();
            this.InitializeFields();
            this.service = new CategoriesService(this.categoriesRepository);
        }

        [Fact]
        public async Task CreateAddsTheCategory()
        {
            await this.service.CreateCategory(
                "Hello",
                "https://miro.medium.com/max/1200/1*mk1-6aYaf_Bes1E3Imhc0A.jpeg",
                "Hallo there");
            var count = this.categoriesRepository.All().Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task CreateSetsAllProperties()
        {
            await this.service.CreateCategory(
                "Hello",
                "hi.jpg",
                "Hallo there");

            var obj = await this.categoriesRepository.All().FirstOrDefaultAsync();

            Assert.Equal("Hello", obj.Title);
            Assert.Equal("hi.jpg", obj.ImageUrl);
            Assert.Equal("Hallo there", obj.Description);
        }

        [Fact]
        public async Task CreateSanitizesTheDescription()
        {
            await this.service.CreateCategory(
                "Hello",
                "hi.jpg",
                "<script>alert(\"RIP\")</script> <p>Hi boys I am new</p>");

            var obj = await this.categoriesRepository.All().FirstOrDefaultAsync();
            var doesItContainScriptOrAlert = obj.Description.Contains("<script>") && obj.Description.Contains("alert(");
            Assert.False(doesItContainScriptOrAlert);
        }

        [Fact]
        public async Task DeleteMethodWorks()
        {
            await this.SeedDatabase();

            await this.service.RemoveCategory(this.testCategory1.Id);

            var count = this.categoriesRepository.All().Count();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task GetByIdWorks()
        {
            await this.SeedDatabase();

            var resultObj = await this.service.GetByIdAsync(this.testCategory1.Id);

            Assert.Equal(this.testCategory1, resultObj);
        }

        [Fact]
        public async Task GetAllMethodReturnsAll()
        {
            await this.SeedDatabase();

            var result = await this.service.GetAll<CategoryListingViewModel>();

            Assert.True(result.Count() == 2);
        }

        [Fact]
        public async Task GetAllMethodSetsTheNumberOfUsersCorrectly()
        {
            await this.SeedDatabase();

            var user = new ApplicationUser
            {
                Id = "Hi",
                Email = "kkfka@aaaabbvv.bg",
                MemberSince = DateTime.Now,
                PasswordHash = "12345",
            };

            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            var post = new Post
            {
                Title = "How to open VS 2019",
                Content = "Hello I am new what is VS 2019 and how to open it?",
                AuthorId = user.Id,
                CategoryId = this.testCategory1.Id,
            };

            var post1 = new Post
            {
                Title = "How to open VS 2019",
                Content = "Hello I am new what is VS 2019 and how to open it?",
                AuthorId = user.Id,
                CategoryId = this.testCategory2.Id,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.AddAsync(post1);

            await this.postsRepository.SaveChangesAsync();

            var result = await this.service.GetAll<CategoryListingViewModel>();
            var isItTrue = result.All(x => x.NumberOfUsers == 1);

            Assert.True(isItTrue);
        }

        [Fact]
        public async Task GetByIdGenericWorks()
        {
            await this.SeedDatabase();

            var expected = new CategoryListingViewModel
            {
                Id = this.testCategory1.Id,
                Title = this.testCategory1.Title,
                Description = this.testCategory1.Description,
                ImageUrl = this.testCategory1.ImageUrl,
                HasRecentPost = true,
            };

            var result = await this.service.GetByIdAsync<CategoryListingViewModel>(this.testCategory1.Id);

            var expectedObj = JsonConvert.SerializeObject(expected);
            var actualResultObj = JsonConvert.SerializeObject(result);

            // I am serializing the objects to ensure that Assert.Equal won't compare any
            // internal properties that I am not trying to compare in this test
            Assert.Equal(expectedObj, actualResultObj);
        }

        [Fact]
        public async Task DoesItExistReturnsTrueIfItExists()
        {
            await this.SeedDatabase();

            var obj = await this.service.DoesItExist(this.testCategory1.Id);

            Assert.True(obj);
        }

        [Fact]
        public async Task DoesItExistReturnsFalseIfItDoesNotExists()
        {
            var obj = await this.service.DoesItExist(this.testCategory2.Id);

            Assert.False(obj);
        }

        [Fact]
        public async Task EditCategoryChangesAllTheNeededProperties()
        {
            await this.SeedDatabase();

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
            await this.SeedDatabase();

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

        private void InitializeDatabaseAndRepositories()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection);
            var dbContext = new ApplicationDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            this.postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
        }

        private void InitializeFields()
        {
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

        private async Task SeedDatabase()
        {
            await this.categoriesRepository.AddAsync(this.testCategory1);
            await this.categoriesRepository.AddAsync(this.testCategory2);
            await this.categoriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"));
    }
}
