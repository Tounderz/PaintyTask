using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Infrastructure.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<UserData?> SignIn(AuthDto authDto);
    Task<UserData?> SignUp(RegisterDto registerDto);
    Task<bool> IsExistEmail(string email);
    Task<bool> IsExistLogin(string login);
}