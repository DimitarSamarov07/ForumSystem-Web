namespace ForumSystem.Data.Models
{
    using System;
    using Common.Models;

    public class PostReply : BaseDeletableModel<int>
    {

        public string Title { get; set; }

        public string Content { get; set; }


        public virtual ApplicationUser User { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
