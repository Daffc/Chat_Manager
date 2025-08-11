using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class ChatRoomMemberRepository : IChatRoomMemberRepository
{
    private readonly AppDbContext _dbContext;

    public ChatRoomMemberRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChatRoomMember> AddAsync(ChatRoomMember chatRoomMember)
    {
        await _dbContext.ChatRoomMember.AddAsync(chatRoomMember);
        await _dbContext.SaveChangesAsync();
        return chatRoomMember;
    }

    public async Task<ChatRoom?> GetByIdAsync(Guid id)
    {
        return await _dbContext.ChatRoom
            .Where(cr => cr.DeletedAt != null)
            .FirstOrDefaultAsync(cr => cr.Id == id);
    }
}