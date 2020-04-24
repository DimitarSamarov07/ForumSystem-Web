using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using ForumSystem.Data.Repositories;
    using Mapping;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Votes;
    using Xunit;

    public class VoteTests
    {
        private IRepository<Vote> votesRepository;
        private IDeletableEntityRepository<ApplicationUser> usersRepository;
        private IDeletableEntityRepository<Post> postsRepository;
        private IDeletableEntityRepository<Category> categoriesRepository;

        private IVoteService service;

        private Vote testUpVote1;
        private Vote testDownVote2;

        private Post testPost1;
        private Post testPost2;

        private ApplicationUser testUser1;
        private ApplicationUser testUser2;

        private Category testCategory1;
        private Category testCategory2;

        public VoteTests()
        {
            this.InitializeDatabaseAndRepositories();
            this.SeedDatabase();
            this.InitializeMapper();

            this.service = new VotesService(this.votesRepository, this.postsRepository);
        }

        [Fact]
        public async Task VoteCreatesNewUpVoteSuccessfullyIfItDoesNotExist()
        {
            await this.service.VoteAsync(this.testPost1.Id, this.testUser1.Id, true);

            var first = await this.votesRepository.All().FirstOrDefaultAsync();

            var isSuccess = first.PostId == this.testPost1.Id
                            &&
                            first.UserId == this.testUser1.Id
                            &&
                            first.VoteType == VoteType.UpVote;

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task VoteCreatesNewDownVoteSuccessfullyIfItDoesNotExist()
        {
            await this.service.VoteAsync(this.testPost1.Id, this.testUser1.Id, false);

            var first = await this.votesRepository.All().FirstOrDefaultAsync();

            var isSuccess = first.PostId == this.testPost1.Id
                            &&
                            first.UserId == this.testUser1.Id
                            &&
                            first.VoteType == VoteType.DownVote;

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task VoteKeepsTheUpVoteIfItExists()
        {
            await this.SeedVotes();

            await this.service.VoteAsync(this.testPost1.Id, this.testUser1.Id, true);

            var first = await this.votesRepository.All()
                .FirstOrDefaultAsync(
                    x => x.PostId == this.testPost1.Id
                         &&
                         x.UserId == this.testUser1.Id);

            var count = await this.votesRepository.All().CountAsync();

            Assert.True(count == 2);
            Assert.True(first.VoteType == VoteType.UpVote);
        }

        [Fact]
        public async Task VoteKeepsTheDownVoteIfItExists()
        {
            await this.SeedVotes();

            await this.service.VoteAsync(this.testPost2.Id, this.testUser2.Id, false);

            var first = await this.votesRepository.All()
                .FirstOrDefaultAsync(
                    x => x.PostId == this.testPost1.Id
                         &&
                         x.UserId == this.testUser1.Id);

            var count = await this.votesRepository.All().CountAsync();

            Assert.True(count == 2);
            Assert.True(first.VoteType == VoteType.UpVote);
        }

        [Fact]
        public async Task GetVotesFromPostWorksAsExpected()
        {
            await this.SeedVotes();
            var user = new ApplicationUser
            {
                Id = "53235532253",
                Email = "test@forumsyhstem.bg",
                MemberSince = DateTime.Now,
                PasswordHash = "1221412214",
            };

            await this.usersRepository.AddAsync(user);
            await this.usersRepository.SaveChangesAsync();

            this.testPost1.Votes.Add(
                new Vote
            {
                User = user,
                VoteType = VoteType.UpVote,
            });

            this.testPost1.Votes.Add(
                new Vote
                {
                    User = this.testUser2,
                    VoteType = VoteType.DownVote,
                });

            this.postsRepository.Update(this.testPost1);
            await this.postsRepository.SaveChangesAsync();
            await this.votesRepository.SaveChangesAsync();

            var result = this.service.GetVotesFromPost(this.testPost1.Id);

            Assert.Equal(1, result);
        }

        private async void SeedDatabase()
        {
            await this.SeedCategories();
            await this.SeedUsers();
            await this.SeedPosts();
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

        private async Task SeedVotes()
        {
            this.testUpVote1 = new Vote
            {
                Post = this.testPost1,
                User = this.testUser1,
                VoteType = VoteType.UpVote,
            };

            this.testDownVote2 = new Vote
            {
                Post = this.testPost2,
                User = this.testUser2,
                VoteType = VoteType.DownVote,
            };

            await this.votesRepository.AddAsync(this.testUpVote1);
            await this.votesRepository.AddAsync(this.testDownVote2);
            await this.votesRepository.SaveChangesAsync();
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
            this.votesRepository = new EfRepository<Vote>(dbContext);
            this.postsRepository = new EfDeletableEntityRepository<Post>(dbContext);
            this.categoriesRepository = new EfDeletableEntityRepository<Category>(dbContext);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"));
    }
}
