using Jobsity.Chat.Contracts.DTOs;
using System.Threading.Tasks;

namespace Jobsity.Chat.Contracts.Interfaces
{
    public interface IIdentityRepository
    {
        Task<UserDto> GetUserAsync(string userId);
    }
}