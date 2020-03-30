namespace ForumSystem.Data.Models
{
    using ForumSystem.Data.Common.Models;

    public class Document : BaseDeletableModel<int>
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
