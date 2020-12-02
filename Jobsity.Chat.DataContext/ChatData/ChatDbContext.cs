using Jobsity.Chat.DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.DataContext.ChatData
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        { }

        public DbSet<ChatConversation> ChatConversation { get; set; }

    }
}
