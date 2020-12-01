using Jobsity.Chat.Contracts.DTOs;
using System.Threading.Tasks;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public interface IIdentityService
    {
        Task<UserDto> GetUser(string userId);
    }
}