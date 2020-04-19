using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Documents
{
    using System.Threading.Tasks;
    using Web.ViewModels.Documents;
    using Web.ViewModels.Home;

    public interface IDocumentService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<T> GetDocumentByTitleAsync<T>(string title);

        Task<T> GetDocumentById<T>(int id);

        Task EditDocumentContentAsync(DocumentEditModel model);

        Task<bool> DoesItExits(int id);

        Task<bool> DoesItExitsByTitle(string title);
    }
}
