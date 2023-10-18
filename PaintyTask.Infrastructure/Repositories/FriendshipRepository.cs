using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaintyTask.Domain.Models;
using PaintyTask.Domain.Models.DTO;
using PaintyTask.Infrastructure.Context;
using PaintyTask.Infrastructure.Repositories.Interfaces;

namespace PaintyTask.Infrastructure.Repositories;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly AppDbContext _context;
    private readonly Mapper _mapper;

    public FriendshipRepository(AppDbContext context, Mapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> IsFriendship(FriendshipDto dto)
    {
        var friendship = await _context.Friendships
            .SingleOrDefaultAsync(i => i.UserSenderId == dto.UserSenderId && i.UserReceiverId == dto.UserReceiverId);
        if (friendship != null && friendship.IsAccepted)
        {
            return true;
        }

        return false;
    }

    public async Task<FriendshipData?> SendFriendshipRequest(FriendshipDto dto)
    {
        var friendship = await _context.Friendships
            .SingleOrDefaultAsync(i => i.UserSenderId == dto.UserSenderId && i.UserReceiverId == dto.UserReceiverId);
        if (friendship != null)
        {
            return null;
        }

        friendship = _mapper.Map<FriendshipData>(dto);

        await _context.Friendships.AddAsync(friendship);
        await _context.SaveChangesAsync();

        return friendship;
    }

    public async Task<FriendshipData?> AcceptFriendshipRequest(FriendshipDto dto)
    {
        var friendship = _mapper.Map<FriendshipData>(dto);
        friendship.IsAccepted = true;

        _context.Friendships.Update(friendship);
        await _context.SaveChangesAsync();

        return friendship;
    }

    public async Task<FriendshipData?> RejectFriendshipRequest(FriendshipDto dto)
    {
        var friendship = await _context.Friendships.SingleOrDefaultAsync(i => i.UserSenderId == dto.UserSenderId && i.UserReceiverId == dto.UserReceiverId);
        if (friendship == null)
        {
            return null;
        }

        _context.Friendships.Remove(friendship);
        await _context.SaveChangesAsync();

        return friendship;
    }
}