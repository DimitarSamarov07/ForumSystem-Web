using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Account;

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Lockout(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            var lockoutEnd = user.LockoutEnd.Value.DateTime;

            var lockoutModel = new LockoutViewModel { LockoutEnd = lockoutEnd };
            return this.View(lockoutModel);
        }
    }
}
