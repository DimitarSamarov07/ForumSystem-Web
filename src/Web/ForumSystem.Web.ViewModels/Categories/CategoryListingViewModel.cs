namespace ForumSystem.Web.ViewModels.Categories
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class CategoryListingViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public bool HasRecentPost { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public int NumberOfPosts { get; set; }

        public int NumberOfUsers { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            DateTime window = DateTime.Now.AddHours(-24);
            configuration.CreateMap<Category, CategoryListingViewModel>()
                .ForMember(
                    x => x.NumberOfPosts,
                    x => x.MapFrom(z => z.Posts.Count))
                .ForMember(
                    x => x.HasRecentPost,
                    x => x.MapFrom(z => z.CreatedOn > window))
                .ForMember(
                    x => x.ImageUrl,
                    x => x.Ignore());
        }
    }
}
