namespace ForumSystem.Data.Models
{
    using System.Collections.Generic;
    using System.Linq;

    using ForumSystem.Data.Common.Models;
    using MoreLinq;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
            this.UsersParticipating = new HashSet<UserCategory>();
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public virtual int NumberOfUsers => this.UsersParticipating.DistinctBy(x => x.User).Count();

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<UserCategory> UsersParticipating { get; set; }

    }
}
