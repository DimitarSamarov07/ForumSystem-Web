namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Common.Models;
    using MoreLinq;

    public class Category : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual int NumberOfUsers => this.UsersParticipating.DistinctBy(x => x.User).Count();

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<UserCategory> UsersParticipating { get; set; }

    }
}
