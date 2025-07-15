namespace Application.DTOs.Requests;

public sealed record RegisterUserRequest(
    string NickName,
    string FirstName,
    string LastName,
    string Email,
    string Password // Plain text
);