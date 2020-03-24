namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common.Models;

    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Upvotes = 0;
            this.Downvotes = 0;
            this.Replies = new HashSet<PostReply>();
        }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int UserId { get; set; }

        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }


        public virtual ICollection<PostReply> Replies { get; set; }
    }
}
