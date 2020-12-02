using AutoMapper;
using Jobsity.Chat.Contracts.DTOs;
using Jobsity.Chat.DataContext.Models;

namespace Jobsity.Chat.Api.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatConversation, MessageDto>()
                .ForMember(m => m.User, opt => opt.MapFrom(src => src.Username))
                .ForMember(m => m.Message, opt => opt.MapFrom(src => src.Message))
                .ReverseMap();
        }
    }
}
