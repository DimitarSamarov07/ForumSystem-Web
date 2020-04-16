using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using Common;
    using Data.Models;
    using Infrastructure.Attributes;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MoreLinq.Extensions;
    using ViewModels.Posts;
    using ViewModels.Profile;

    public class ProfileController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Auth(GlobalConstants.AdministratorRoleName)]
        public async Task<IActionResult> Index()
        {
            var profiles = await this.userManager.GetUsersAsync<ApplicationUser, ProfileModel>();
            var model = new ProfileListModel
            {
                Profiles = profiles
                    .Select(
                        c =>
                    {
                        c.IsAdmin = this.userManager.IsUserAdmin(c.UserId).Result;
                        return c;
                    })

                    .ToList(),
            };

            return this.View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                return this.NotFound();
            }

            var isUserAdmin = await this.userManager.IsUserAdmin(user);
            var model = new ProfileModel()
            {
                UserId = user.Id,
                Username = user.UserName,
                UserKarmaPoints = user.KarmaPoints,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = isUserAdmin,
            };

            return this.View(model);
        }
    }
}
