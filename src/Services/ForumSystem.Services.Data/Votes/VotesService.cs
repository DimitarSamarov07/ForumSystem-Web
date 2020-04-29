namespace ForumSystem.Services.Data.Votes
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ForumSystem.Data.Common.Repositories;
    using ForumSystem.Data.Models;
    using ForumSystem.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;

    public class VotesService : IVoteService
    {
        private readonly IRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public VotesService(IRepository<Vote> votesRepository, IDeletableEntityRepository<Post> postsRepository)
        {
            this.votesRepository = votesRepository;
            this.postsRepository = postsRepository;
        }

        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);
            var post = this.postsRepository.All().Include(x => x.Author).FirstOrDefault(x => x.Id == postId);
            if (vote != null)
            {
                if (isUpVote)
                {
                    post.Author.KarmaPoints++;
                    vote.VoteType = VoteType.UpVote;
                }
                else
                {
                    // Yes it is possible for an author
                    // to have negative karma points
                    post.Author.KarmaPoints--;
                    vote.VoteType = VoteType.DownVote;
                }
            }
            else
            {
                vote = new Vote
                {
                    PostId = postId,
                    UserId = userId,
                    VoteType = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };
                if (vote.VoteType == VoteType.UpVote)
                {
                    post.Author.KarmaPoints++;
                }
                else
                {
                    post.Author.KarmaPoints--;
                }

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }

        public int GetVotesFromPost(int postId)
        {
            var votesValue = this.votesRepository.All()
                .Where(x => x.PostId == postId)
                .ToList();

            var obj = votesValue
                .Sum(x => (int)x.VoteType);
            return obj;
        }
    }
}
