using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Common.Models;

    public class Reply : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public Reply InnerReply { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
