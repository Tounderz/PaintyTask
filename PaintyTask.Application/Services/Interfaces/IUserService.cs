using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models.ResponseModels;

namespace PaintyTask.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserData?> GetUserById(int id);
    Task<UserData?> GetUserByLoginOrEmail(string loginOrEmail);
    Task<IReadOnlyCollection<PhotoData>?> GetPhotos(int userId);
    IEnumerable<UserResponse>? GetUsersWithReceivedFriendshipRequests(int receiverId);
    Task<PhotoData?> CreatePhoto(PhotoDto dto);
}