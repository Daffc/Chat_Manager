using MediatR;

namespace Application.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<bool>;