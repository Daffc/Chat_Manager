using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }
    public string NickName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}