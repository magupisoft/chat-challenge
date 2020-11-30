using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.UI.ChatHub;
using Jobsity.Chat.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Jobsity.Chat.UI.Controllers
{
    [Authorize]
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IHubContext<JobSityChatHub> _hubContext;

        private readonly Data.ApplicationDbContext _db;

        public ChatController(IHubContext<JobSityChatHub> hubContext, Data.ApplicationDbContext db)
        {
            _hubContext = hubContext;
            _db = db;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(MessageDto msg)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = _db.Users.FirstOrDefault(x => x.Id == currentUserId);
            await _hubContext.Clients.All.SendAsync("ReceiveChatMessage", currentUser.UserName, msg.Message);
            return Ok();
        }
    }
}
