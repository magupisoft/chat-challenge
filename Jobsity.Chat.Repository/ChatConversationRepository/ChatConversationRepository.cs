using AutoMapper;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.DataContext.ChatData;
using Jobsity.Chat.DataContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jobsity.Chat.Repository.ChatConversationRepository
{
    public class ChatConversationRepository : IChatConversationRepository
    {
        private readonly ChatDbContext _db;
        private readonly IMapper _mapper;
        public ChatConversationRepository(ChatDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IList<MessageDto>> GetConversationAsync(int numMsgs)
        {
            var dbResult = await _db.ChatConversation.OrderBy(x => x.Timestamp).Take(numMsgs).ToListAsync();
            IList<MessageDto> result = _mapper.Map<IList<ChatConversation>, IList<MessageDto>>(dbResult);

            return result;
        }

        public async Task<bool> SaveConversationMessage(MessageDto msg)
        {
            var chatConversation = _mapper.Map<MessageDto, ChatConversation>(msg);
            chatConversation.Timestamp = DateTime.UtcNow;
            _db.ChatConversation.Add(chatConversation);
            var insertedId = await _db.SaveChangesAsync();
            return insertedId != 0;
        }
    }
}
