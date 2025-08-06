using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ChatRoomMember
{
    // Properties
    public ChatRoomMember(Guid userId, Guid chatRoomId)
    {
        UserId = userId;
        ChatRoomId = chatRoomId;
    }
    public Guid UserId { get; private set; }
    public Guid ChatRoomId { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public DateTime? LeftAt { get; private set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ChatRoom? ChatRoom { get; set; }
}