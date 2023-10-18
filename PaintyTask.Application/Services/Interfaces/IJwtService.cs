namespace PaintyTask.Application.Services.Interfaces;

public interface IJwtService
{
    string GenerateJwt(string email, int userId);
    int GetUserIdFromToken(string token);
}