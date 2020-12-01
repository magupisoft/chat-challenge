using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.UI.ChatHub;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Jobsity.Chat.UI.Controllers
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<JobSityChatHub> _hubContext;
        private readonly IIdentityService _identityService;
        public ChatController(IHubContext<JobSityChatHub> hubContext, IIdentityService identityService)
        {
            _hubContext = hubContext;
            _identityService = identityService;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(MessageDto msg)
        {
            var user = await _identityService.GetUser(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _hubContext.Clients.All.SendAsync("ReceiveChatMessage", user.UserName, msg.Message);
            return Ok();
        }
    }
}
