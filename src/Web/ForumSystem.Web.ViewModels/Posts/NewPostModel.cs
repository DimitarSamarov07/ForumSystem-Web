namespace ForumSystem.Web.ViewModels.Posts
{
    public class NewPostModel
    {
        public string ForumName { get; set; }

        public string AuthorName { get; set; }

        public string ForumImageUrl { get; set; }

        public int ForumId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
    }
}
