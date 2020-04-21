namespace ForumSystem.Web.ViewModels.Posts
{
    public class NewPostModel
    {
        public string CategoryName { get; set; }

        public string AuthorName { get; set; }

        public string CategoryImageUrl { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }
    }
}
