namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Common.Models;

    public class Post : BaseDeletableModel<int>
    {
        public Post()
        {
            this.Upvotes = 0;
            this.Downvotes = 0;
            this.Replies = new HashSet<Reply>();
        }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string Content { get; set; }

        public int Upvotes { get; set; }

        public int Downvotes { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual Category Category { get; set; }

        public int CategoryId { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }

    }
}
