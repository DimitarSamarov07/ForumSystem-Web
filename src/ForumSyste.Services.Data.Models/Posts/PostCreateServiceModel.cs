namespace ForumSystem.Services.Data.Models.Posts
{
    public class PostCreateServiceModel
    {
        public string Title { get; set; }

        public string Content { get; set; }


        public string AuthorId { get; set; }

        public int CategoryId { get; set; }
    }
}
