using AutoMapper;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models.ResponseModels;

namespace PaintyTask.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<RegisterDto, UserData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SentFriendshipRequests, opt => opt.Ignore())
            .ForMember(dest => dest.ReceivedFriendshipRequests, opt => opt.Ignore())
            .ForMember(dest => dest.Photos, opt => opt.Ignore())
            .ForMember(user =>
                    user.Password, opt =>
                    opt.MapFrom(dto => BCrypt.Net.BCrypt.HashPassword(dto.Password)))
            .ForAllMembers(options => options.Condition((dto, user, srcMember) => srcMember != null));

        CreateMap<UserData, UserResponse>()
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        CreateMap<string, PhotoDto>()
            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
        CreateMap<int, PhotoDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src));

        CreateMap<PhotoDto, PhotoData>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        CreateMap<int, FriendshipDto>()
            .ForMember(dest => dest.UserSenderId, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.UserReceiverId, opt => opt.Ignore());
        CreateMap<FriendshipDto, FriendshipData>()
            .ForMember(dest => dest.IsAccepted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.UserSender, opt => opt.Ignore())
            .ForMember(dest => dest.UserReceiver, opt => opt.Ignore());
    }
}