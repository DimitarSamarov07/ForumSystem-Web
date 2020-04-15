using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSystem.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Votes;
    using ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IVoteService votesService;

        public VotesController(
            UserManager<ApplicationUser> userManager,
            IVoteService votesService)
        {
            this.userManager = userManager;
            this.votesService = votesService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.votesService.VoteAsync(input.PostId, userId, input.IsUpVote);
            var votes = this.votesService.GetVotesFromPost(input.PostId);
            return new VoteResponseModel { VotesCount = votes };
        }
    }
}
