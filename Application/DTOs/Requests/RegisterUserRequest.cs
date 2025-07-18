using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public sealed record RegisterUserRequest(
    [Required(ErrorMessage = "Nickname is required")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Nickname must be between 3-20 character")]
    string NickName,

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "First name must be between 3-50 character")]
    string FirstName,

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Last name must be between 3-50 character")]
    string LastName,

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    string Email,

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must have between 8-20 characters with at least 1 uppercase, 1 lowercase, 1 number and 1 special character")]
    string Password
);