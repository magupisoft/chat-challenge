using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobsity.Chat.Service.ConversationService
{
    public class ConversationService : IConversationService
    {
        private readonly IChatConversationRepository _chatConversationRepository;

        public ConversationService(IChatConversationRepository chatConversationRepository)
        {
            _chatConversationRepository = chatConversationRepository;
        }

        public Task<IList<MessageDto>> GetConversationAsync(int numMsgs) => _chatConversationRepository.GetConversationAsync(numMsgs);

        public Task<bool> SaveConversationMessage(MessageDto msg) => _chatConversationRepository.SaveConversationMessage(msg);
    }
}
