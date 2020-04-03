using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    public class PostReplyReport : Report
    {
        public Reply Reply { get; set; }

        public int ReplyId { get; set; }
    }
}
