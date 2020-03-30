namespace ForumSystem.Web.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using ForumSystem.Web.ViewModels.AccountViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToLocal(returnUrl);
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            // Clear the existing external cookie to ensure a clean login process
            await this.signInManager.SignOutAsync();
            var result = await this.signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                model.RememberMe,
                false);


            if (result.Succeeded)
            {
                return this.RedirectToLocal(returnUrl);
            }

            if (result.IsLockedOut)
            {
                var user = await this.userManager.FindByNameAsync(model.UserName);
                var lockoutEnd = user.LockoutEnd.Value.DateTime;

                var lockoutModel = new LockoutViewModel { LockoutEnd = lockoutEnd };
                return this.RedirectToAction(nameof(this.Lockout), lockoutModel);
            }

            this.ModelState.AddModelError(string.Empty, "Wrong credentials!");
            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Redirect("/");
            }

            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, MemberSince = DateTime.Now };
            var result = await this.userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return this.View();
            }

            await this.signInManager.SignInAsync(user, isPersistent: false);
            return this.RedirectToLocal(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult Lockout(LockoutViewModel model)
        {
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
