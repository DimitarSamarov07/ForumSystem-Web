using Microsoft.AspNetCore.Mvc;

namespace ForumSystem.Web.Controllers
{
    [Route("Error")]
    public class ErrorController : BaseController
    {
        [Route("404")]
        public IActionResult Error404()
        {
            return this.View();
        }

        [Route("401")]
        public IActionResult Error401()
        {
            return this.View();
        }

        [Route("500")]
        public IActionResult Error500()
        {
            return this.View();
        }
    }
}