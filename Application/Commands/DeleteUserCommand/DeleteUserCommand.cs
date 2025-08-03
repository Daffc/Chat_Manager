using MediatR;
using System.Security.Claims;

namespace Application.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId, ClaimsPrincipal User) : IRequest<bool>;