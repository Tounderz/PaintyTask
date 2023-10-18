using Microsoft.AspNetCore.Http;
using PaintyTask.Application.Services.Interfaces;
using PaintyTask.Application.Validators.Interfaces;
using PaintyTask.Domain.Exceptions;
using PaintyTask.Domain.Models.DTO;
using System.ComponentModel.DataAnnotations;

namespace PaintyTask.Application.Validators;

public class Validate : IValidate
{
    private readonly IAuthService _authService;

    public Validate(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task ValidateCreateUser(RegisterDto registerDto)
    {
        if (registerDto == null || string.IsNullOrEmpty(registerDto.Email)
            || string.IsNullOrEmpty(registerDto.Password)
            || string.IsNullOrEmpty(registerDto.Login)
            || string.IsNullOrEmpty(registerDto.Name))
        {
            throw new ValidateException("All fields required");
        }

        if (!IsValidEmail(registerDto.Email))
        {
            throw new ValidateException("Invalid email format");
        }

        if (await _authService.IsExistEmail(registerDto.Email))
        {
            throw new ValidateException("User with this email is exist");
        }

        if (await _authService.IsExistLogin(registerDto.Login))
        {
            throw new ValidateException("User with this login is exist");
        }
    }

    public void ValidateAuth(AuthDto authDto)
    {
        if (authDto == null
            || string.IsNullOrEmpty(authDto.Login)
            || string.IsNullOrEmpty(authDto.Password))
        {
            throw new ValidateException("All fields required");
        }
    }

    public void ValidateCreatePhoto(IFormFile formFile)
    {
        if (formFile == null)
        {
            throw new ValidateException("Choose a file");
        }
    }

    public void ValidateSendFriendship(string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            throw new ValidateException("All fields required");
        }
    }

    public void ValidateIsPositive(int id)
    {
        if (id < 1)
        {
            throw new ValidateException("Select a user to confirm friendship with");
        }
    }

    private bool IsValidEmail(string email)
    {
        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
}