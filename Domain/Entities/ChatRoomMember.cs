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
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }

    // Navigation properties
    public virtual User? User { get; set; }
    public virtual ChatRoom? ChatRoom { get; set; }
}