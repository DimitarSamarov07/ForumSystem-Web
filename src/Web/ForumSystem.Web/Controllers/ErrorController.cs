using System.Collections.Generic;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ForumSystem.Web.Controllers
{
    [Route("Error")]
    public class ErrorController : BaseController
    {
        [Route("404")]
        public IActionResult Error()
        {
            return this.View();
        }
    }
}