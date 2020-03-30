using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    using Common.Models;

    public class Reply : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser User { get; set; }

        public int UserId { get; set; }

        public Reply InnerReply { get; set; }

        public ICollection<PostReply> PostReplies { get; set; }
    }
}
