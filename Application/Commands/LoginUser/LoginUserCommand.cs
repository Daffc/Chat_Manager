using MediatR;
using Application.DTOs.Responses;

namespace Application.Commands.LoginUser;
public record LoginUserCommand(string Email, string Password) : IRequest<AuthResponse>;