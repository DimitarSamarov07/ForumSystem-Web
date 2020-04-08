using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Replies
{
    using System.Threading.Tasks;
    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Web.ViewModels.Reply;

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

        public async Task RemoveReply(int id)
        {
            var replyToRemove = await this.repliesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            this.repliesRepository.Delete(replyToRemove);
            await this.repliesRepository.SaveChangesAsync();
        }
    }
}
