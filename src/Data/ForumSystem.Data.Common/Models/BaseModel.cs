namespace ForumSystem.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using ForumSystem.Data.Common.Models;

    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
