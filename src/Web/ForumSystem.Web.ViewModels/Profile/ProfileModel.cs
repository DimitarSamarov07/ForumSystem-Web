namespace ForumSystem.Web.ViewModels.Profile
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class ProfileModel
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string UserKarmaPoints { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime MemberSince { get; set; }

        public IFormFile ImageUpload { get; set; }

        public bool IsAdmin { get; set; }
    }
}
