namespace ForumSystem.Web.ViewModels.Categories
{
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
            configuration.CreateMap<Category, CategoryListingViewModel>()
                .ForMember(
                    x => x.NumberOfPosts,
                    x => x.MapFrom(z => z.Posts.Count));
        }
    }
}
