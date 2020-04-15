namespace ForumSystem.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using Reply;

    public class PostIndexModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorImageUrl { get; set; }

        public int AuthorRating { get; set; }

        public DateTime Created { get; set; }

        public string PostContent { get; set; }

        public bool IsAuthorAdmin { get; set; }

        public bool IsCurrentUserAuthorOrAdmin { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<PostReplyModel> Replies { get; set; }

        public int TotalVotes { get; private set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostIndexModel>()
                .ForMember(
                    x => x.AuthorId,
                    x => x
                        .MapFrom(z => z.AuthorId.ToString()))
                .ForMember(
                    x => x.AuthorName,
                    x => x.MapFrom(z => z.Author.UserName))
                .ForMember(
                    x => x.AuthorImageUrl,
                    x => x.MapFrom(z => z.Author.ProfileImageUrl))
                .ForMember(
                    x => x.AuthorRating,
                    x => x.MapFrom(z => z.Author.KarmaPoints))
                .ForMember(
                    x => x.Created,
                    x => x.MapFrom(z => z.CreatedOn))
                .ForMember(
                    x => x.PostContent,
                    x => x.MapFrom(z => z.Content))
                .ForMember(
                    x => x.CategoryId,
                    x => x.MapFrom(z => z.CategoryId))
                .ForMember(
                    x => x.CategoryName,
                    x => x.MapFrom(z => z.Category.Title))
                .ForMember(
                    x => x.TotalVotes,
                    x => x.MapFrom(z => z.Votes.Sum(v => (int)v.VoteType)));
        }
    }
}
