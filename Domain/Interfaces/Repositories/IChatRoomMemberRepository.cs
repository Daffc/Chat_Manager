using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomMemberRepository
{
    Task<ChatRoomMember> AddAsync(ChatRoomMember chatRoomMember);
}