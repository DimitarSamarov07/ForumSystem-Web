namespace ForumSystem.Services.Data.Votes
{
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;

    public class VotesService : IVoteService
    {
        private readonly IRepository<Vote> votesRepository;

        public VotesService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);
            if (vote != null)
            {
                vote.VoteType = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }
            else
            {
                vote = new Vote
                {
                    PostId = postId,
                    UserId = userId,
                    VoteType = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }

        public int GetVotesFromPost(int postId)
        {
            var votesValue = this.votesRepository.All()
                .Where(x => x.PostId == postId)
                .Sum(x => (int)x.VoteType);

            return votesValue;
        }
    }
}
