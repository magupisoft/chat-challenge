
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Jobsity.Chat.UI.ChatHub
{
    public class JobSityChatHub : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveChatMessage", user, message);
        }
    }
}
