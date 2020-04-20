using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PostController : AdministrationController
    {
        public IActionResult Index()
        {
            return this.RedirectToAction("Index", "Category");
        }
    }
}
