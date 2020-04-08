namespace ForumSystem.Services.Data.Replies
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;

    public interface IReplyService
    {
        Task CreateReplyAsync(Reply reply);

        Task RemoveReply(int id);
    }
}
