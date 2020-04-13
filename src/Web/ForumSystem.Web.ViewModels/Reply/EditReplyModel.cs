using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Reply
{
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class EditReplyModel : IMapFrom<Reply>, IHaveCustomMappings
    {
        public int ReplyId { get; set; }

        public int PostId { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Reply, EditReplyModel>()
                .ForMember(
                    x => x.ReplyId,
                    x => x.MapFrom(z => z.Id))
                .ForMember(
                    x => x.AuthorName,
                    x => x.MapFrom(z => z.Author.UserName));
        }
    }
}
