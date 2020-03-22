namespace ForumSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using Common.Models;

    public class Post : BaseDeletableModel<int>
    {

        public string Title { get; set; }

        public string Content { get; set; }


        public virtual ApplicationUser User { get; set; }

        public virtual Category Category { get; set; }

        public virtual IEnumerable<PostReply> Replies { get; set; }


    }
}
 