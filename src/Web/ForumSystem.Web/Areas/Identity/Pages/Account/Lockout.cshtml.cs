namespace ForumSystem.Web.Areas.Identity.Pages.Account
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<LoginModel> logger;

        public LockoutModel(UserManager<ApplicationUser> userManager, ILogger<LoginModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        [DataType(DataType.DateTime)]
        public DateTime LockoutEnd { get; set; }

        public async void OnGetUser(string id)
        {
            logger.LogWarning("{0}", id);
            var user = await this.userManager.FindByIdAsync(id);
            var lockoutEnd = user.LockoutEnd.Value.DateTime;

            this.LockoutEnd = lockoutEnd;
        }
    }
}
