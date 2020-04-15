namespace ForumSystem.Web.ViewModels.Profile
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Services.Mapping;

    public class ProfileModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public int UserKarmaPoints { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime MemberSince { get; set; }

        public bool IsAdmin { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, ProfileModel>()
                .ForMember(
                    x => x.UserKarmaPoints,
                    x => x.MapFrom(z => z.KarmaPoints))
                .ForMember(
                    x => x.UserId,
                    x => x.MapFrom(z => z.Id));
        }
    }
}
