
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class ChatRoom
{
    public ChatRoom(Guid ownerId, string name)
    {
        OwnerId = ownerId;
        Name = name;
    }
    // Properties
    [Key]
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public User? Owner { get; private set; }
    public IReadOnlyCollection<ChatRoomMember> Members => _members.AsReadOnly();
    public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
    private readonly List<ChatRoomMember> _members = new();
    private readonly List<Message> _messages = new();
}