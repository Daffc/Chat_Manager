using Application.DTOs.Responses;
using MediatR;

namespace Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string NickName,
    string FirstName,
    string LastName,
    string Email,
    string Password
) : IRequest<UserResponse>;