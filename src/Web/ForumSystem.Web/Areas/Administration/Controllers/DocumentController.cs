namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ForumSystem.Services.Data.Documents;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Documents;

    public class DocumentController : AdministrationController
    {
        private readonly IDocumentService documentsService;

        public DocumentController(IDocumentService documentsService)
        {
            this.documentsService = documentsService;
        }

        public async Task<IActionResult> Index()
        {
            var documents = await this.documentsService.GetAll<DocumentIndexModel>();

            var model = new DocumentAdminPanelModel
            {
                Documents = documents,
            };

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

            return this.RedirectToAction("Index", "Document");
        }
    }
}
