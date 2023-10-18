using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Infrastructure.Repositories.Interfaces;

public interface IFriendshipRepository
{
    Task<bool> IsFriendship(FriendshipDto dto);
    Task<FriendshipData?> SendFriendshipRequest(FriendshipDto dto);
    Task<FriendshipData?> AcceptFriendshipRequest(FriendshipDto dto);
    Task<FriendshipData?> RejectFriendshipRequest(FriendshipDto dto);
}