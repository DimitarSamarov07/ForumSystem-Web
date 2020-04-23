namespace ForumSystem.Services.Data.Replies
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Services.Mapping;
    using ForumSystem.Web.ViewModels.Reply;
    using Ganss.XSS;
    using Microsoft.EntityFrameworkCore;

    public class RepliesService : IReplyService
    {
        private readonly IDeletableEntityRepository<Reply> repliesRepository;

        public RepliesService(IDeletableEntityRepository<Reply> repliesRepository)
        {
            this.repliesRepository = repliesRepository;
        }

        public async Task CreateReplyAsync(Reply reply)
        {
            await this.repliesRepository.AddAsync(reply);
            await this.repliesRepository.SaveChangesAsync();
        }

        public async Task EditReplyContent(EditReplyModel model)
        {
            var reply = await this.GetReplyById(model.ReplyId);
            var content = new HtmlSanitizer().Sanitize(model.Content);
            reply.Content = content;
            this.repliesRepository.Update(reply);
            await this.repliesRepository.SaveChangesAsync();
        }

        public async Task<T> GetReplyById<T>(int id)
        {
            var obj = await this.repliesRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            return obj;
        }

        public async Task RemoveReplyAsync(int id)
        {
            var replyToRemove = await this.repliesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (replyToRemove == null)
            {
                return;
            }

            this.repliesRepository.Delete(replyToRemove);
            await this.repliesRepository.SaveChangesAsync();
        }

        public async Task<bool> DoesItExist(int id)
        {
            var obj = await this.GetReplyById(id);

            return obj != null;
        }

        private async Task<Reply> GetReplyById(int id)
        {
            var reply = await this.repliesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            return reply;
        }
    }
}
