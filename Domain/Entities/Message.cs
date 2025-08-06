using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Message
{
    public Message(Guid chatRoomId, Guid senderId, string content)
    {
        ChatRoomId = chatRoomId;
        SenderId = senderId;
        Content = content;
    }

    // Properties
    [Key]
    public Guid Id { get; private set; }
    public Guid ChatRoomId { get; private set; }
    public Guid SenderId { get; private set; }
    public string Content { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual User? Sender { get; set; }
    public virtual ChatRoom? ChatRoom { get; set; }
}