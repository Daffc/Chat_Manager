using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomRepository
{
    Task<ChatRoom> AddAsync(ChatRoom chatRoom);
}