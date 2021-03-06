﻿namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common.Models;

    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Replies = new HashSet<Reply>();
            this.Votes = new HashSet<Vote>();
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }

        public virtual Category Category { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
