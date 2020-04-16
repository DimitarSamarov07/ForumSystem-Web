using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Documents;
    using ViewModels.Documents;
    using ViewModels.Home;

    public class DocumentController : BaseController
    {
        private readonly IDocumentService documentsService;

        public DocumentController(IDocumentService documentsService)
        {
            this.documentsService = documentsService;
        }

        public async Task<IActionResult> Index(string title)
        {
            if (!await this.documentsService.DoesItExitsByTitle(title))
            {
                return this.NotFound();
            }

            var model = await this.documentsService.GetDocumentByTitleAsync<DocumentIndexModel>(title);

            return this.View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.documentsService.DoesItExits(id))
            {
                return this.NotFound();
            }

            var model = await this.documentsService.GetDocumentById<DocumentEditModel>(id);
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DocumentEditModel model)
        {
            if (!await this.documentsService.DoesItExits(model.DocumentId))
            {
                return this.NotFound();
            }

            await this.documentsService.EditDocumentContentAsync(model);

            return this.RedirectToAction("Index", "Document", new { title = model.Title });
        }
    }
}
