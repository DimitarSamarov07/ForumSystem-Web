using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Common.Models;

    public class Reply : BaseDeletableModel<int>
    {
        public string Content { get; set; }

        public ApplicationUser Author { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
