using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Data.Models
{
    public class UserCategory
    {
        public ApplicationUser User { get; set; }

        public int UserId { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }
    }
}
