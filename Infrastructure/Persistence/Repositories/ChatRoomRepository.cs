using Domain.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly AppDbContext _dbContext;

    public ChatRoomRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ChatRoom> AddAsync(ChatRoom chatRoom)
    {
        await _dbContext.ChatRoom.AddAsync(chatRoom);
        await _dbContext.SaveChangesAsync();
        return chatRoom;
    }
}