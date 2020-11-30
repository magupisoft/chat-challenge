using ChatClient.ChatHub;
using Jobsity.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatClient.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<JobSityChatHub> _hubContext;
        public ChatController(IHubContext<JobSityChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Route("send")]
        [HttpPost]
        public IActionResult SendRequest(MessageDto msg)
        {
            _hubContext.Clients.All.SendAsync("ReceiveChatMessage", msg.User, msg.Message);
            return Ok();
        }
    }
}
