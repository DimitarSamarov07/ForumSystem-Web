namespace ForumSystem.Data.Models
{
    using System;
    using Common.Models;

    public class PostReply : BaseDeletableModel<int>
    {
        public int ReplyId { get; set; }

        public Reply Reply { get; set; }

        public int PostId { get; set; }

        public virtual Post Post { get; set; }
    }
}
