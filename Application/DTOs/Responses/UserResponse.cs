namespace Application.DTOs.Responses;

public sealed record UserResponse(
    Guid Id,
    string NickName,
    string FirstName,
    string LastName,
    string Email
);