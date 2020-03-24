namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using ForumSystem.Services.Data;

    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        //public IActionResult Index()
        //{
        //    var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
        //    return this.View(viewModel);
        //}
    }
}
