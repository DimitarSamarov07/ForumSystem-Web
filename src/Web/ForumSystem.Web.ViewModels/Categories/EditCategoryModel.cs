using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Categories
{
    using AutoMapper;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Services.Mapping;

    public class EditCategoryModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public string CategoryDescription { get; set; }

        public IFormFile NewImage { get; set; }

        public string ImageUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Category, EditCategoryModel>()
                .ForMember(
                    x => x.CategoryId,
                    x => x.MapFrom(z => z.Id))
                .ForMember(
                    x => x.CategoryTitle,
                    x => x.MapFrom(z => z.Title))
                .ForMember(
                    x => x.CategoryDescription,
                    x => x.MapFrom(z => z.Description))
                .ForMember(
                    x => x.ImageUrl,
                    x => x.Ignore());
        }
    }
}
