using Jobsity.Chat.Contracts.Commands;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.Contracts.Messaging;
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
        private readonly IRabbitMqService _rabbitMqService;
        public ChatController(IHubContext<JobSityChatHub> hubContext, IIdentityService identityService, IRabbitMqService rabbitMqService)
        {
            _hubContext = hubContext;
            _identityService = identityService;
            _rabbitMqService = rabbitMqService;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendRequest(MessageDto msg)
        {
            var user = await _identityService.GetUser(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (msg.Message.StartsWith("/stock="))
            {
                var symbol = msg.Message.Split('=')[1];
                _rabbitMqService.OnQuoteDataReceived += new NotifyCallerDelegate(OnQuoteReceived);
                _rabbitMqService.AskForQuote(symbol);
                await _hubContext.Clients.All.SendAsync("ReceiveChatMessage", "Bot", $"You have asked for the following stock: {symbol} ...please wait a moment while getting your quote");
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("ReceiveChatMessage", user.UserName, msg.Message);
            }
            return Ok();
        }

        private void OnQuoteReceived(MessageEventArgs<StockQuoteCommand> args)
        {
            if (args.Message == null) return;
            _hubContext.Clients.All.SendAsync("ReceiveChatMessage", "Bot", $"{args.Message.Symbol} quote is ${args.Message.Price} per share").ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
}
