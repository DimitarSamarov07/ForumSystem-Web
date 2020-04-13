namespace ForumSystem.Services.Data.Replies
{
    using System.Threading.Tasks;

    using ForumSystem.Data.Models;
    using Web.ViewModels.Reply;

    public interface IReplyService
    {
        Task CreateReplyAsync(Reply reply);

        Task EditReplyContent(EditReplyModel model);

        Task<T> GetReplyById<T>(int id);

        Task RemoveReplyAsync(int id);
    }
}
