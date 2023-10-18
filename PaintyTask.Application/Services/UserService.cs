using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models.ResponseModels;
using PaintyTask.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaintyTask.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserData?> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<UserData?> GetUserByLoginOrEmail(string loginOrEmail)
    {
        var emailAttribute = new EmailAddressAttribute();
        return !emailAttribute.IsValid(loginOrEmail)
                    ? await _userRepository.GetUserByLogin(loginOrEmail)
                    : await _userRepository.GetUserByEmail(loginOrEmail);
    }

    public async Task<IReadOnlyCollection<PhotoData>?> GetPhotos(int userId)
    {
        return await _userRepository.GetPhotos(userId);
    }

    public IEnumerable<UserResponse>? GetUsersWithReceivedFriendshipRequests(int receiverId)
    {
        return _userRepository.GetUsersWithReceivedFriendshipRequests(receiverId);
    }

    public async Task<PhotoData?> CreatePhoto(PhotoDto dto)
    {
        return await _userRepository.CreatePhoto(dto);
    }
}