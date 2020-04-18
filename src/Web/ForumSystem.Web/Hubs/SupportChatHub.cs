namespace ForumSystem.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

    public class SupportChatHub : Hub
    {
        //public async Task Send(string message)
        //{
        //    await this.Clients.Client()
        //    await this.Clients.All.SendAsync(
        //        "NewMessage",
        //        new Message { User = this.Context.User.Identity.Name, Text = message, });
        //}
    }
}
