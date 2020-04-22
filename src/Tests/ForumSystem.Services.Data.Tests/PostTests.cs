namespace ForumSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using ForumSystem.Services.Data.Categories;
    using ForumSystem.Services.Data.Posts;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Posts;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class PostTests
    {
        private PostsService postsService;

        private EfDeletableEntityRepository<Category> categoriesRepository;
        private EfDeletableEntityRepository<Post> postsRepository;
        private EfDeletableEntityRepository<ApplicationUser> usersRepository;

        private Post testPost1;
        private Post testPost2;
        private Post testPost3;
        private Post testPost4;

        private ApplicationUser testUser1;
        private ApplicationUser testUser2;

        private Category testCategory1;
        private Category testCategory2;

        public PostTests()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection);
            var dbContext = new ApplicationDbContext(options.Options);

            dbContext.Database.EnsureCreated();

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
                Author = this.testUser1,
                Content = "Hi brothers how are you?",
                Category = this.testCategory1,
            };

            this.testPost2 = new Post
            {
                Title = "Test Post 12345678",
                Author = this.testUser2,
                Content = "How to use PC thingy?",
                Category = this.testCategory2,
            };

            this.testPost3 = new Post
            {
                Title = "Selling bushes",
                Author = this.testUser1,
                Content = "The 2$ per bush. Always the best quality :)",
                Category = this.testCategory1,
            };

            this.testPost4 = new Post
            {
                Title = "Kali Linux custom terminal theme",
                Author = this.testUser2,
                Content = "Hello, I am trying to change my Kali Linux terminal theme. I know that I have" +
                          "to change the .bashrc file using nano, or cat if I am total noob but I can't " +
                          "find a good theme. Pls help!",
                Category = this.testCategory2,
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

        [Fact]
        public async Task GetAllFromCategoryAsEnumerableReturnsOnlyFromTheProperCategory()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetAllFromCategory<PostListingViewModel>(this.testCategory1.Id);

            var isSuccessful = result.Count() == 1;

            Assert.True(isSuccessful);
        }

        [Fact]
        public async Task GetByIdGenericWorksAsExpected()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetByIdAsync<PostListingViewModel>(this.testPost2.Id);

            Assert.True(result.Id.Equals(this.testPost2.Id));
        }

        [Fact]
        public async Task DoesItExistsReturnsTrueIfThereAreMatchingEntities()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService.DoesItExits(this.testPost2.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesItExistsReturnsFalseIfThereAreNoMatchingEntities()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService.DoesItExits(123);

            Assert.False(result);
        }

        [Fact]
        public async Task EditPostContentEditsTheContent()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var model = new EditPostModel
            {
                PostId = this.testPost1.Id,
                Content = "Hi guys I am a noobie.",
                PostTitle = this.testPost1.Title,
            };

            await this.postsService.EditPostContent(model);

            var result = this.testPost1.Content == model.Content;

            Assert.True(result);
        }

        [Fact]
        public async Task EditPostContentSanitizesTheContent()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var model = new EditPostModel
            {
                PostId = this.testPost1.Id,
                Content = "<script>alert(\"Hacked!\")</script> <p>Hi guys I am a noobie.</p>",
                PostTitle = this.testPost1.Title,
            };

            await this.postsService.EditPostContent(model);

            var result = !this.testPost1.Content.Contains("<script>")
                         &&
                         this.testPost1.Content.Contains("<p>");

            Assert.True(result);
        }

        [Fact]
        public async Task GetLatestPostsOrdersByDescendingByDate()
        {
            this.testPost1.CreatedOn = DateTime.Now.AddDays(-1);
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService.GetLatestPosts<PostListingViewModel>(2);
            var isSuccess = result.First().Id == this.testPost2.Id;

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task GetMostPopularPostsOrdersByDescendingReplies()
        {
            this.testPost1.Replies = Enumerable.Repeat(
                    new Reply
                    {
                        Content = "Hello",
                    },
                    5)
                .ToList();

            this.testPost2.Replies = Enumerable.Repeat(
                    new Reply
                    {
                        Content = "Hello",
                    },
                    3)
                .ToList();

            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await
                this.postsService.GetMostPopularPosts<PostListingViewModel>(2);

            Assert.True(result.First().Id == this.testPost1.Id);
        }

        [Fact]
        public async Task GetMostPopularPostsGetsTheGivenCountOfPosts()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetMostPopularPosts<PostListingViewModel>(3);

            Assert.True(result.Count() == 3);
        }

        [Fact]
        public async Task GetLatestPostsTakesTheCorrectCount()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService.GetLatestPosts<PostListingViewModel>(1);
            var isSuccess = result.Count() == 1;
            Assert.True(isSuccess);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("       ")]
        public async Task GetFilteredPostsReturnsNullIfSearchQueryIsNullOrWhitespace(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = this.postsService.GetFilteredPosts<PostListingViewModel>(query);

            Assert.True(result == null);
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData("hi")]
        [InlineData("pC")]
        public async Task GetFilteredPostsIgnoresCasing(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = this.postsService.GetFilteredPosts<PostListingViewModel>(query);

            Assert.True(result.Any());
        }

        [Theory]
        [InlineData("Test post")]
        [InlineData("1234")]
        public async Task GetFilteredPostsSearchesInTitle(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = this.postsService.GetFilteredPosts<PostListingViewModel>(query);

            Assert.True(result.Any());
        }

        [Theory]
        [InlineData("brothers")]
        [InlineData("PC")]
        [InlineData("thingy")]
        public async Task GetFilteredPostsSearchesInDescription(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();

            var result = this.postsService.GetFilteredPosts<PostListingViewModel>(query);

            Assert.True(result.Any());
        }

        [Fact]
        public async Task GetFilteredPostsFromCategorySearchesOnlyInTheGivenCategory()
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetFilteredPosts<PostListingViewModel>(this.testCategory2.Id, "to");

            var isSuccess = result.All(
                x => x.Id == this.testPost2.Id
                     || x.Id == this.testPost4.Id);

            Assert.True(isSuccess);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("       ")]
        public async Task GetFilteredPostsFormCategoryReturnsNullIfSearchQueryIsNullOrWhitespace(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetFilteredPosts<PostListingViewModel>(this.testCategory1.Id, query);

            Assert.True(result == null);
        }

        [Theory]
        [InlineData("kali")]
        [InlineData("LiNuX")]
        [InlineData(".BASHRC")]
        [InlineData("nAno, oR Cat")]
        public async Task GetFilteredPostsFromCategoryIgnoresCasing(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetFilteredPosts<PostListingViewModel>(this.testCategory2.Id, query);

            Assert.True(result.Count() == 1);
        }

        [Theory]
        [InlineData("Test post")]
        [InlineData("1234")]
        public async Task GetFilteredPostsFromCategorySearchesInTitle(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetFilteredPosts<PostListingViewModel>(this.testCategory1.Id, query);

            Assert.True(result.Count() == 1);
        }

        [Theory]
        [InlineData("bashrc")]
        [InlineData("PC")]
        [InlineData("thingy")]
        public async Task GetFilteredPostsFromCategorySearchesInDescription(string query)
        {
            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.AddAsync(this.testPost3);
            await this.postsRepository.AddAsync(this.testPost4);
            await this.postsRepository.SaveChangesAsync();

            var result = await this.postsService
                .GetFilteredPosts<PostListingViewModel>(this.testCategory2.Id, query);

            Assert.True(result.Count() == 1);
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
