using Microsoft.AspNetCore.Http;
using PaintyTask.Domain.Models.DTO;

namespace PaintyTask.Application.Validators.Interfaces;

public interface IValidate
{
    Task ValidateCreateUser(RegisterDto registerDto);
    void ValidateCreatePhoto(IFormFile formFile);
    void ValidateSendFriendship(string login);
    void ValidateIsPositive(int id);
    void ValidateAuth(AuthDto authDto);
}