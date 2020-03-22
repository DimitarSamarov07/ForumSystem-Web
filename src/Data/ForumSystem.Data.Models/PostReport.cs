using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    public class PostReport : Report
    {
        public Post ReportedPost { get; set; }

        public int ReportedPostId { get; set; }
    }
}
