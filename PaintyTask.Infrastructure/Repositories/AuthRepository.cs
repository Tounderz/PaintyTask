using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Infrastructure.Context;
using PaintyTask.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PaintyTask.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    private readonly Mapper _mapper;

    public AuthRepository(AppDbContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserData?> SignIn(AuthDto authDto)
    {
        var emailAttribute = new EmailAddressAttribute();
        var user = emailAttribute.IsValid(authDto.Login)
            ? await _context.Users.FirstOrDefaultAsync(i =>
                i.Email.ToLower() == authDto.Login.ToLower())
            : await _context.Users.FirstOrDefaultAsync(i =>
                i.Login.ToLower() == authDto.Login.ToLower());

        return user == null || !BCrypt.Net.BCrypt.Verify(authDto.Password, user.Password) ? null : user;
    }

    public async Task<UserData?> SignUp(RegisterDto registerDto)
    {
        var user = _mapper.Map<UserData>(registerDto);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<bool> IsExistEmail(string email)
    {
        return await _context.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> IsExistLogin(string login)
    {
        return await _context.Users.AnyAsync(user => user.Login.ToLower() == login.ToLower());
    }
}