using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Profile
{
    using Microsoft.AspNetCore.Http;

    public class ChangeProfilePictureInputModel
    {
        public string UserId { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
