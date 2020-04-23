namespace ForumSystem.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using ForumSystem.Services.Data.Replies;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Reply;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using Xunit;

    public class ReplyTests
    {
        private IDeletableEntityRepository<ApplicationUser> usersRepository;
        private IDeletableEntityRepository<Post> postsRepository;
        private IDeletableEntityRepository<Category> categoriesRepository;
        private IDeletableEntityRepository<Reply> repliesRepository;

        private IReplyService service;

        private Post testPost1;
        private Post testPost2;

        private ApplicationUser testUser1;
        private ApplicationUser testUser2;

        private Category testCategory1;
        private Category testCategory2;

        private Reply testReply1;
        private Reply testReply2;

        public ReplyTests()
        {
            this.InitializeDatabaseAndRepositories();
            this.SeedDatabase();
            this.InitializeMapper();

            this.service = new RepliesService(this.repliesRepository);
        }

        [Fact]
        public async Task CreateReplyCreatesReply()
        {
            await this.service.CreateReplyAsync(
                new Reply
                {
                    Content = "Hiiiii there",
                    Author = this.testUser1,
                    Post = this.testPost1,
                });

            var count = await this.repliesRepository.All().CountAsync();
            Assert.True(count == 3);
        }

        [Fact]
        public async Task CreateReplySetsAllTheProperties()
        {
            var obj = new Reply
            {
                Content = "Hiiiii there",
                Author = this.testUser1,
                Post = this.testPost1,
            };

            await this.service.CreateReplyAsync(obj);

            var last = await this.repliesRepository.All().OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            Assert.Equal(obj.Post, last.Post);
            Assert.Equal(obj.Author, last.Author);
            Assert.Equal(obj.Post, last.Post);
        }

        [Fact]
        public async Task RemoveReplyRemovesTheEntity()
        {
            await this.service.RemoveReplyAsync(this.testReply2.Id);

            var count = await this.repliesRepository.All().CountAsync();

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RemoveReplyDoesNotRemoveAnythingOrThrowsErrorIfTheEntityIsNotFound()
        {
            await this.service.RemoveReplyAsync(12345);

            var count = await this.repliesRepository.All().CountAsync();

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task EditReplyContentEditsTheContent()
        {
            var obj = new EditReplyModel
            {
                ReplyId = this.testReply1.Id,
                PostId = this.testReply1.PostId,
                Content = "New content 1234",
            };

            await this.service.EditReplyContent(obj);

            var replyContent = this.testReply1.Content;

            Assert.True(replyContent == obj.Content);
        }

        [Fact]
        public async Task EditReplyContentSanitizesTheContent()
        {
            var obj = new EditReplyModel
            {
                ReplyId = this.testReply1.Id,
                PostId = this.testReply1.PostId,
                Content = "<script>alert(\"Hacked!\")</script><p>New content 1234</p>",
            };

            await this.service.EditReplyContent(obj);

            var replyContent = this.testReply1.Content;
            var isSuccess = !replyContent.Contains("<script>") && replyContent.Contains("<p>");

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task GetByIdGetsTheRightEntity()
        {
            var model = new EditReplyModel
            {
                AuthorName = this.testPost2.Author.UserName,
                ReplyId = this.testReply2.Id,
                PostId = this.testReply2.PostId,
                Content = this.testReply2.Content,
            };

            var result = await this.service.GetReplyById<EditReplyModel>(this.testReply2.Id);

            var expected = JsonConvert.SerializeObject(model);
            var actual = JsonConvert.SerializeObject(result);

            // I am serializing the objects to ensure that it Assert.Equal won't compare any
            // internal properties that I am not trying to compare in this test
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetByIdReturnsNullIfTheEntityIsNotFound()
        {
            var result = await this.service.GetReplyById<EditReplyModel>(12345);

            Assert.Null(result);
        }

        [Fact]
        public async Task DoesItExistReturnsTrueIfTheEntityExists()
        {
            var result = await this.service.DoesItExist(this.testReply1.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesItExistReturnsFalseIfTheEntityDoesNotExist()
        {
            var result = await this.service.DoesItExist(12345);

            Assert.False(result);
        }

        private async void SeedDatabase()
        {
            await this.SeedCategories();
            await this.SeedUsers();
            await this.SeedPosts();
            await this.SeedReplies();
        }

        private async Task SeedCategories()
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

        private async Task SeedUsers()
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

        private async Task SeedPosts()
        {
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

            await this.postsRepository.AddAsync(this.testPost1);
            await this.postsRepository.AddAsync(this.testPost2);
            await this.postsRepository.SaveChangesAsync();
        }

        private async Task SeedReplies()
        {
            this.testReply1 = new Reply
            {
                Content = "Pretty good actually. What" +
                          "about you :v ",
                Post = this.testPost1,
                Author = this.testUser1,
            };

            this.testReply2 = new Reply
            {
                Content = "I am just scrolling through" +
                          "the different versions of " +
                          "Tetris" +
                          "" +
                          "I love it!",
                Post = this.testPost2,
                Author = this.testUser2,
            };

            await this.repliesRepository.AddAsync(this.testReply1);
            await this.repliesRepository.AddAsync(this.testReply2);
            await this.repliesRepository.SaveChangesAsync();
        }

        private void InitializeDatabaseAndRepositories()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection);
            var dbContext = new ApplicationDbContext(options.Options);

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            this.usersRepository = new EfDeletableEntityRepository<ApplicationUser>(dbContext);
            this.repliesRepository = new EfDeletableEntityRepository<Reply>(dbContext);
            this.postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"));
    }
}
