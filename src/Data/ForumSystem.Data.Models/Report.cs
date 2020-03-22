using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using ForumSystem.Common;

    public class Report : BaseDeletableModel<int>
    {
        public string Title { get; set; }


        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        [MinLength(ValidationConstants.DescriptionMinLength)]
        public string Description { get; set; }
    }
}
