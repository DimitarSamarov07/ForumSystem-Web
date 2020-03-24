using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Posts
{
    using System.Linq;
    using AutoMapper;
    using Categories;
    using Data.Models;
    using Services.Mapping;

    public class PostListingViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public double AuthorRating { get; set; }

        public int RepliesCount { get; set; }

        public string DatePosted { get; set; }

        public CategoryListingViewModel Category { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostListingViewModel>()
                .ForMember(
                    x => x.AuthorId,
                    z => z.MapFrom(x => x.User.UserName))
                .ForMember(
                    x => x.RepliesCount,
                    z => z.MapFrom(x => x.Replies.Count(c => !c.IsDeleted)))
                .ForMember(
                    x => x.DatePosted,
                    x=>x.MapFrom(z => z.CreatedOn));

        }
    }
}
