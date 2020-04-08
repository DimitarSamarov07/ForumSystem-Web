using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common.Models;
    using Enums;

    public class Vote : BaseModel<int>
    {
        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        public VoteType VoteType { get; set; }
    }
}
