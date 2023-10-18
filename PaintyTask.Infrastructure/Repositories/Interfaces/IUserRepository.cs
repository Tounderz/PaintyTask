using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models.ResponseModels;

namespace PaintyTask.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserData?> GetUserById(int id);
    Task<UserData?> GetUserByLogin(string login);
    Task<UserData?> GetUserByEmail(string email);
    Task<IReadOnlyCollection<PhotoData>?> GetPhotos(int userId);
    IEnumerable<UserResponse>? GetUsersWithReceivedFriendshipRequests(int receiverId);
    Task<PhotoData?> CreatePhoto(PhotoDto dto);
}