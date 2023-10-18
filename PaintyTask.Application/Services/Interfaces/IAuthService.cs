using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models;

namespace PaintyTask.Application.Services.Interfaces;

public interface IAuthService
{
    Task<UserData?> SignIn(AuthDto authDto);
    Task<UserData?> SignUp(RegisterDto registerDto);
    Task<bool> IsExistEmail(string email);
    Task<bool> IsExistLogin(string login);
}