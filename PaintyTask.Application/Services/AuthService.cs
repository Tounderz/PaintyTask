using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Infrastructure.Repositories.Interfaces;

namespace PaintyTask.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<UserData?> SignIn(AuthDto authDto)
    {

        return await _authRepository.SignIn(authDto);
    }

    public async Task<UserData?> SignUp(RegisterDto registerDto)
    {

        return await _authRepository.SignUp(registerDto);
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _authRepository.IsExistEmail(email);
    }

    public async Task<bool> IsExistLogin(string login)
    {
        return await _authRepository.IsExistLogin(login);
    }
}