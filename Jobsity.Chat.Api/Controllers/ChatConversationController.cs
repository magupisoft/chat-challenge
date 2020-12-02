using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Jobsity.Chat.Api.Controllers
{
    /// <summary>
    /// Test
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [ApiVersion("1.0")]
    [Route("api/chat/conversation")]
    public class ChatConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        /// <summary>
        /// Chat Conversation Ctor.
        /// </summary>
        /// <param name="conversationService"></param>
        public ChatConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        /// <summary>
        /// Returns the last numbers of chats messages
        /// </summary>
        /// <param name="numMsgs">Number of message to retrieve</param>
        /// <returns>Chat Conversation Messages</returns>
        /// <response code="200">Chat Conversation found.</response>
        /// <response code="404">Chat Conversation is not found.</response>
        [HttpGet("/{numMsgs}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetConversation(int numMsgs = 50)
        {
            return Ok(await _conversationService.GetConversationAsync(numMsgs));
        }

        /// <summary>
        /// Saves Chat Message
        /// </summary>
        /// <returns></returns>
        /// <response code="202">Chat Conversation Message saved correctly.</response>
        /// <response code="500">Error while saving Chat Conversation Message.</response>
        [HttpPost("/save")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> SaveChatMessage([FromBody]MessageDto msg)
        {
            try
            {
                await _conversationService.SaveConversationMessage(msg);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status422UnprocessableEntity, ex.Message);
            }
            return NoContent();
        }
    }
}
