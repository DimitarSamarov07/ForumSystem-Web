namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Linq;

    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Categories;

    public class PostListingViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public int AuthorKarmaPoints { get; set; }

        public bool IsAuthorAdmin { get; set; }

        public int RepliesCount { get; set; }

        public string DatePosted { get; set; }

        public CategoryListingViewModel Category { get; set; }

        public int CategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostListingViewModel>()
                .ForMember(
                    x => x.AuthorId,
                    z => z.MapFrom(x => x.Author.Id))
                .ForMember(
                    x => x.AuthorName,
                    z => z.MapFrom(x => x.Author.UserName))
                .ForMember(
                    x => x.AuthorKarmaPoints,
                    z => z.MapFrom(x => x.Author.KarmaPoints))
                .ForMember(
                    x => x.RepliesCount,
                    z => z.MapFrom(x => x.Replies.Count(c => !c.IsDeleted)))
                .ForMember(
                    x => x.DatePosted,
                    x => x.MapFrom(z => z.CreatedOn))
                .ForMember(
                    x => x.CategoryId,
                    x => x.MapFrom(z => z.CategoryId));
        }
    }
}
