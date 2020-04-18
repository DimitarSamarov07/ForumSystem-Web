namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using ForumSystem.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    [Route("/Hello/[controller]")]
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Dashboard()
        {
            return this.View();
        }

        //public IActionResult Index()
        //{
        //    var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
        //    return this.View(viewModel);
        //}
    }
}
