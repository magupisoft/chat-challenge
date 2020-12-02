using System;

namespace Jobsity.Chat.DataContext.Models
{
    public class ChatConversation
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
