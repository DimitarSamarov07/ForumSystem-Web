using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Categories;
    using ForumSystem.Data;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Posts;
    using Web.ViewModels.Posts;
    using Xunit;

    public class PostTests
    {
        private PostsService postsService;

        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;

        private Post testPost1;
        private Post testPost2;

        private ApplicationUser testUser1;
        private ApplicationUser testUser2;

        private Category testCategory1;
        private Category testCategory2;

        public PostTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
            var dbContext = new ApplicationDbContext(options.Options);

            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
            this.postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);

            this.postsService = new PostsService(
                this.postsRepository,
                new CategoriesService(this.categoriesRepository));

            this.SeedUsers();
            this.SeedCategories();
            this.InitializeMapper();

            this.testPost1 = new Post
            {
                Title = "Test Post 1234",
                AuthorId = this.testUser1.Id,
                Content = "Hi brothers how are you?",
                CreatedOn = DateTime.Now,
                CategoryId = this.testCategory1.Id,
            };

            this.testPost2 = new Post
            {
                Title = "Test Post 12345678",
                AuthorId = this.testUser2.Id,
                Content = "How to use PC thingy?",
                CreatedOn = DateTime.Now,
                CategoryId = this.testCategory2.Id,
            };
        }

        [Fact]
        public async Task CreateWorks()
        {
            var model1 = new NewPostModel
            {
                Title = this.testPost1.Title,
                Content = this.testPost1.Content,
                AuthorId = this.testUser1.Id,
                CategoryId = this.testCategory1.Id,
            };

            var model2 = new NewPostModel
            {
                Title = this.testPost2.Title,
                Content = this.testPost2.Content,
                AuthorId = this.testUser2.Id,
                CategoryId = this.testCategory2.Id,
            };

            await this.postsService.CreatePostAsync(model1);
            await this.postsService.CreatePostAsync(model2);

            var isSuccessful = this.postsRepository.All().Count() == 2;

            Assert.True(isSuccessful);
        }

        [Fact]
        public async Task RemoveWorks()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            await this.postsService.RemovePostAsync(this.testPost1.Id);

            var isRemoved = this.postsRepository.All().Count() == 1;

            Assert.True(isRemoved);
        }

        [Fact]
        public async Task GetAllFromCategoryAsQueryableReturnsOnlyFromTheProperCategory()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = this.postsService
                .GetAllFromCategoryАsQueryable<PostListingViewModel>(this.testCategory1.Id);

            var isSuccessful = result.Count() == 1;

            Assert.True(isSuccessful);
        }

        private async void SeedUsers()
        {
            this.testUser1 = new ApplicationUser
            {
                Id = "Hi",
                Email = "kkfka@aaaabbvv.bg",
                MemberSince = DateTime.Now,
                PasswordHash = "12345",
            };

            this.testUser2 = new ApplicationUser
            {
                Id = "Test Hi 1",
                Email = "example@example.com",
                MemberSince = DateTime.Now,
                PasswordHash = "1332142",
            };

            await this.usersRepository.AddAsync(this.testUser1);
            await this.usersRepository.AddAsync(this.testUser2);

            await this.usersRepository.SaveChangesAsync();
        }

        private async void SeedCategories()
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

            await this.categoriesRepository.AddAsync(this.testCategory1);
            await this.categoriesRepository.AddAsync(this.testCategory2);
            await this.categoriesRepository.SaveChangesAsync();
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"));
    }
}
