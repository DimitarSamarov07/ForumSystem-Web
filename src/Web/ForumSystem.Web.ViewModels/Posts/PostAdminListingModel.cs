using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Linq;
    using AutoMapper;
    using Data.Models;
    using Reply;
    using Services.Mapping;

    public class PostAdminListingModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<PostReplyModel> Replies { get; set; }

        public int TotalVotes { get; private set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostIndexModel>()
                .ForMember(
                    x => x.AuthorId,
                    x => x.MapFrom(z => z.AuthorId.ToString()))
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
