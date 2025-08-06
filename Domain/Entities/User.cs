using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class User
{
    public User(string nickName, string firstName, string lastName, string email, string password)
    {
        NickName = nickName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    // Properties
    [Key]
    public Guid Id { get; private set; }
    public string NickName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    public virtual ICollection<ChatRoom> OwnedChatRooms { get; private set; } = new List<ChatRoom>();
    public virtual ICollection<ChatRoomMember> ChatRoomMemberships { get; private set; } = new List<ChatRoomMember>();
    public virtual ICollection<Message> SentMessages { get; private set; } = new List<Message>();

}