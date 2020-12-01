using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using System.Threading.Tasks;

namespace Jobsity.Chat.Service.IdentityService
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityRepository _identityRepository;

        public IdentityService(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public Task<UserDto> GetUser(string userId) => _identityRepository.GetUserAsync(userId);

    }
}
