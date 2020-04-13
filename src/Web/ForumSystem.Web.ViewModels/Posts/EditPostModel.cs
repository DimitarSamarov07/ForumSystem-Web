namespace ForumSystem.Web.ViewModels.Posts
{
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class EditPostModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string Content { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, EditPostModel>()
                .ForMember(
                    x => x.PostId,
                    x => x.MapFrom(z => z.Id))
                .ForMember(
                    x => x.PostTitle,
                    x => x.MapFrom(z => z.Title));
        }
    }
}
