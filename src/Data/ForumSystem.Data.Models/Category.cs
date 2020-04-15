namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using ForumSystem.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]
        public int NumberOfUsers { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

    }
}
