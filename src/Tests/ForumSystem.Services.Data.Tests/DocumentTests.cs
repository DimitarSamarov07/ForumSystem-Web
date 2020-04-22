namespace ForumSystem.Services.Data.Tests
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Documents;
    using ForumSystem.Data;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Repositories;
    using Mapping;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Documents;
    using Xunit;

    public class DocumentTests
    {
        private IDeletableEntityRepository<Document> documentsRepository;
        private IDocumentService service;

        private Document testDocument1;
        private Document testDocument2;

        public DocumentTests()
        {
            this.InitializeDatabaseAndRepositories();
            this.InitializeMapper();
            this.InitializeFields();
            this.SeedDatabase();
            this.service = new DocumentsService(this.documentsRepository);
        }

        [Fact]
        public async Task GetAllWorksReturnsAll()
        {
            var result = await this.service.GetAll<DocumentIndexModel>();

            Assert.True(result.Count() == 2);
        }

        [Fact]
        public async Task GetDocumentByTitleReturnsTheCorrectModel()
        {
            var result = await this.service
                .GetDocumentByTitleAsync<DocumentIndexModel>(this.testDocument1.Title);

            Assert.True(result.DocumentId == this.testDocument1.Id);
        }

        [Fact]
        public async Task GetDocumentByIdReturnsTheCorrectModel()
        {
            var result = await this.service
                .GetDocumentById<DocumentIndexModel>(this.testDocument2.Id);

            Assert.True(result.DocumentId == this.testDocument2.Id);
        }

        [Fact]
        public async Task EditDocumentContentEditsTheContent()
        {
            var model = new DocumentEditModel
            {
                DocumentId = this.testDocument1.Id,
                Title = this.testDocument1.Title,
                Content = "New Content Here :)",
            };

            await this.service.EditDocumentContentAsync(model);

            var isSuccess = this.testDocument1.Content == model.Content;

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task EditDocumentContentSanitizesTheContent()
        {
            var model = new DocumentEditModel
            {
                DocumentId = this.testDocument1.Id,
                Title = this.testDocument1.Title,
                Content = "<script>alert(\"Ohh you got destroyed\")</script>" +
                          " <p>New Content Here :)</p>",
            };

            await this.service.EditDocumentContentAsync(model);

            var isSuccess = !this.testDocument1.Content.Contains("<script>")
                            &&
                            this.testDocument1.Content.Contains("<p>");

            Assert.True(isSuccess);
        }

        [Fact]
        public async Task DoesItExistReturnsTrueIfTheEntityExists()
        {
            var result = await this.service.DoesItExist(this.testDocument2.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesItExistReturnsFalseIfTheEntityDoesNotExist()
        {
            var result = await this.service.DoesItExist(1234);

            Assert.False(result);
        }

        [Fact]
        public async Task DoesItExistByTitleReturnsTrueIfTheEntityExists()
        {
            var result = await this.service.DoesItExistByTitle(this.testDocument1.Title);

            Assert.True(result);
        }

        [Fact]
        public async Task DoesItExistByTitleReturnsFalseIfTheEntityDoesNotExists()
        {
            var result = await this.service.DoesItExistByTitle("Warframe Empyrean");

            Assert.False(result);
        }


        private async void SeedDatabase()
        {
            await this.documentsRepository.AddAsync(this.testDocument1);
            await this.documentsRepository.AddAsync(this.testDocument2);
            await this.documentsRepository.SaveChangesAsync();
        }

        private void InitializeFields()
        {
            this.testDocument1 = new Document
            {
                Title = "Privacy",
                Content = "Who even reads this LOL :D",
            };

            this.testDocument2 = new Document
            {
                Title = "Rules",
                Content = "TBH the only rule you should follow is " +
                          "not to annoy the admin or the things will went" +
                          "uhh bad :D",
            };
        }

        private void InitializeDatabaseAndRepositories()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection);
            var dbContext = new ApplicationDbContext(options.Options);

            dbContext.Database.EnsureCreated();

            this.documentsRepository = new EfDeletableEntityRepository<Document>(dbContext);
        }

        private void InitializeMapper() => AutoMapperConfig.
            RegisterMappings(Assembly.Load("ForumSystem.Web.ViewModels"));
    }
}
