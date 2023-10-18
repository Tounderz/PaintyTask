using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Infrastructure.Repositories.Interfaces;

namespace PaintyTask.Application.Services;

public class FriendshipService : IFriendshipService
{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendshipService(IFriendshipRepository friendshipRepository)
    {
        _friendshipRepository = friendshipRepository;
    }

    public async Task<bool> IsFriendship(FriendshipDto dto)
    {
        return await _friendshipRepository.IsFriendship(dto);
    }

    public async Task<FriendshipData?> SendFriendshipRequest(FriendshipDto dto)
    {
        return await _friendshipRepository.SendFriendshipRequest(dto);
    }

    public async Task<FriendshipData?> AcceptFriendshipRequest(FriendshipDto dto)
    {
        return await _friendshipRepository.AcceptFriendshipRequest(dto);
    }

    public async Task<FriendshipData?> RejectFriendshipRequest(FriendshipDto dto)
    {
        return await _friendshipRepository.RejectFriendshipRequest(dto);
    }
}