namespace ForumSystem.Web.ViewModels.Reply
{
    using System;
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class PostReplyModel : IMapFrom<Reply>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorImageUrl { get; set; }

        public int AuthorRating { get; set; }

        public DateTime Created { get; set; }

        public string ReplyContent { get; set; }

        public bool IsAuthorAdmin { get; set; }

        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public string CategoryName { get; set; }

        public string CategoryImageUrl { get; set; }

        public int CategoryId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reply, PostReplyModel>()
                .ForMember(
                    x => x.AuthorId,
                    x => x
                        .MapFrom(z => z.UserId.ToString()))
                .ForMember(
                    x => x.AuthorName,
                    x => x.MapFrom(z => z.User.UserName))
                .ForMember(
                    x => x.AuthorImageUrl,
                    x => x.MapFrom(z => z.User.ProfileImageUrl))
                .ForMember(
                    x => x.AuthorRating,
                    x => x.MapFrom(z => z.User.Rating))
                .ForMember(
                    x => x.Created,
                    x => x.MapFrom(z => z.CreatedOn))
                .ForMember(
                    x => x.PostContent,
                    x => x.MapFrom(z => z.Post.Content))
                .ForMember(
                    x => x.CategoryId,
                    x => x.MapFrom(z => z.Post.CategoryId))
                .ForMember(
                    x => x.CategoryName,
                    x => x.MapFrom(z => z.Post.Category.Title))
                .ForMember(
                    x => x.ReplyContent,
                    x => x.MapFrom(z => z.Content));
        }


    }
}
