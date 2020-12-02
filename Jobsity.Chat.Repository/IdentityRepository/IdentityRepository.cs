using AutoMapper;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.Contracts.Interfaces;
using Jobsity.Chat.DataContext.IdentityData;
using System.Threading.Tasks;

namespace Jobsity.Chat.Repository.IdentityRepository
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IdentityDbContext _db;
        private readonly IMapper _mapper;
        public IdentityRepository(IdentityDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserAsync(string userId)
        {
            var user = await _db.Users.FindAsync(userId);
            return _mapper.Map<UserDto>(user);
        }
    }
}
