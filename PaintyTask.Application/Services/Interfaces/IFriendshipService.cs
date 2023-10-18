using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Application.Services.Interfaces;

public interface IFriendshipService
{
    Task<bool> IsFriendship(FriendshipDto dto);
    Task<FriendshipData?> SendFriendshipRequest(FriendshipDto dto);
    Task<FriendshipData?> AcceptFriendshipRequest(FriendshipDto dto);
    Task<FriendshipData?> RejectFriendshipRequest(FriendshipDto dto);
}
