using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Domain.Models.ResponseModels;
using PaintyTask.Infrastructure.Context;
using PaintyTask.Infrastructure.Repositories.Interfaces;

namespace PaintyTask.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly Mapper _mapper;

    public UserRepository(AppDbContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserData?> GetUserById(int id)
    {
        return await _context.Users
            .Include(i => i.Photos)
            .SingleOrDefaultAsync(i =>i.Id == id);
    }

    public async Task<UserData?> GetUserByEmail(string email)
    {
        return await _context.Users
            .Include(i => i.Photos)
            .SingleOrDefaultAsync(i => i.Email.ToLower() == email.ToLower());
    }

    public async Task<UserData?> GetUserByLogin(string login)
    {
        return await _context.Users
            .Include(i => i.Photos)
            .SingleOrDefaultAsync(i => i.Login.ToLower() == login.ToLower());
    }

    public async Task<IReadOnlyCollection<PhotoData>?> GetPhotos(int userId)
    {
        var user = await GetUserById(userId);
        return user == null 
            || user.Photos == null 
            || !user.Photos.Any() 
                ? null 
                : user.Photos;
    }

    public IEnumerable<UserResponse>? GetUsersWithReceivedFriendshipRequests(int receiverId)
    {
        var users = _context.Friendships
            .Where(f => f.UserReceiverId == receiverId && !f.IsAccepted)
            .Select(f => f.UserSender);

        return users == null || !users.Any()
            ? null 
            : _mapper.Map<IEnumerable<UserResponse>>(users);
    }

    public async Task<PhotoData?> CreatePhoto(PhotoDto dto)
    {
        var user = await GetUserById(dto.UserId);
        if (user == null)
        {
            return null;
        }

        var photo = _mapper.Map<PhotoData>(dto);

        await _context.Photos.AddAsync(photo);
        await _context.SaveChangesAsync();

        user.AddPhoto(photo);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        
        return photo;
    }
}