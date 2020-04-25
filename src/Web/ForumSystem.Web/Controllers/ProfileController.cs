using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using CloudinaryDotNet;
    using Common;
    using Data.Models;
    using Infrastructure.Attributes;
    using Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MoreLinq.Extensions;
    using ViewModels.Posts;
    using ViewModels.Profile;

    public class ProfileController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Cloudinary cloudinary;

        public ProfileController(UserManager<ApplicationUser> userManager, Cloudinary cloudinary)
        {
            this.userManager = userManager;
            this.cloudinary = cloudinary;
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
            var model = new ProfileModel
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

        [HttpPost]
        public async Task<IActionResult> ChangeProfileImage(ChangeProfilePictureInputModel input)
        {
            var user = await this.userManager.FindByIdAsync(input.UserId);

            if (this.User.Identity.Name != user.UserName)
            {
                return this.NotFound();
            }

            user.ProfileImageUrl = await this.Upload(input.ImageUpload);
            await this.userManager.UpdateAsync(user);

            return this.RedirectToAction("Details", "Profile", new { id = input.UserId });
        }

        private async Task<string> Upload(IFormFile file)
        {
            var uri = await this.cloudinary.UploadAsync(file);
            return uri;
        }
    }
}
