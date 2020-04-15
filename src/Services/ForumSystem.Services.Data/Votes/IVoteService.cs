using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Services.Data.Votes
{
    using System.Threading.Tasks;
    using Web.ViewModels.Votes;

    public interface IVoteService
    {
        Task VoteAsync(int postId, string userId, bool isUpVote);

        int GetVotesFromPost(int postId);
    }
}
