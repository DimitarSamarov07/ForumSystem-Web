using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Documents
{
    using System.Linq;
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using Mapping;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Documents;
    using Web.ViewModels.Home;

    public class DocumentsService : IDocumentService
    {
        private readonly IDeletableEntityRepository<Document> documentsRepository;

        public DocumentsService(IDeletableEntityRepository<Document> documentsRepository)
        {
            this.documentsRepository = documentsRepository;
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

            document.Content = model.Content;

            await this.documentsRepository.SaveChangesAsync();
        }

        public async Task<bool> DoesItExits(int id)
        {
            var obj = await this.documentsRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            return obj != null;
        }
    }
}
