namespace ForumSystem.Web.ViewModels.Documents
{
    using AutoMapper;
    using Data.Models;
    using Services.Mapping;

    public class DocumentEditModel : IMapFrom<Document>, IHaveCustomMappings
    {
        public int DocumentId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Document, DocumentEditModel>()
                .ForMember(
                    x => x.DocumentId,
                    x => x.MapFrom(z => z.Id));
        }
    }
}
