using System;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public interface IMessageBrokerSettings
    {
        public string HostName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TimeSpan RecoveryInterval { get; set; }
    }
}
