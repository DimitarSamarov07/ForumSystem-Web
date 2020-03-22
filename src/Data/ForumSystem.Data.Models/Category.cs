namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }


        public string ImageUrl { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }

    }
}
