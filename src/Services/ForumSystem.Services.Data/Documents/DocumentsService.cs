namespace ForumSystem.Services.Data.Documents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Documents;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;

    public class DocumentsService : IDocumentService
    {
        private readonly IDeletableEntityRepository<Document> documentsRepository;

        public DocumentsService(IDeletableEntityRepository<Document> documentsRepository)
        {
            this.documentsRepository = documentsRepository;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            var obj = await this.documentsRepository.All()
                 .To<T>()
                 .ToListAsync();

            return obj;
        }

        public async Task<T> GetDocumentByTitleAsync<T>(string title)
        {
            var document = await this.documentsRepository
                .All()
                .Where(x => x.Title == title)
                .To<T>()
                .FirstOrDefaultAsync();

            return document;
        }

        public async Task<T> GetDocumentById<T>(int id)
        {
            var document = await this.documentsRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return document;
        }

        public async Task EditDocumentContentAsync(DocumentEditModel model)
        {
            var document = await this.documentsRepository
                .All()
                .Where(x => x.Id == model.DocumentId)
                .FirstOrDefaultAsync();

            document.Content = new HtmlSanitizer().Sanitize(model.Content);

            await this.documentsRepository.SaveChangesAsync();
        }

        public async Task<bool> DoesItExist(int id)
        {
            var obj = await this.documentsRepository.All()
                .FirstOrDefaultAsync(x => x.Id == id);
            return obj != null;
        }

        public async Task<bool> DoesItExistByTitle(string title)
        {
            var obj = await this.documentsRepository.All().FirstOrDefaultAsync(x => x.Title == title);

            return obj != null;
        }
    }
}
