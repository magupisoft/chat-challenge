﻿using Jobsity.Chat.Contracts.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public interface IChatConversationRepository
    {
        Task<IList<MessageDto>> GetConversationAsync(int numMsgs);
        Task<bool> SaveConversationMessage(MessageDto msg);
    }
}
