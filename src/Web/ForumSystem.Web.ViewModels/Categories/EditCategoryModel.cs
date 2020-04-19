using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Services.Mapping;

    public class EditCategoryModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "Length cannot be shorter than 6 characters!")]
        [MaxLength(40, ErrorMessage = "Length cannot be longer than 40 characters!")]
        public string CategoryTitle { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(50, ErrorMessage = "Length cannot be shorter than 50 characters!")]
        [MaxLength(300, ErrorMessage = "Length cannot be longer than 300 characters!")]
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
                    x => x.Ignore())
                .ForMember(
                    x => x.NewImage,
                    x => x.Ignore());
        }
    }
}
