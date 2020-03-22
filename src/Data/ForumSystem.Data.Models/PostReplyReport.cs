using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    public class PostReplyReport : Report
    {
        public PostReply PostReply { get; set; }

        public int PostReplyId { get; set; }
    }
}
