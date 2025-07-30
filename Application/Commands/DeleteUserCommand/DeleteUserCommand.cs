using MediatR;

namespace Application.Commands.DeleteUser;

public record DeleteUserCommand(Guid userId) : IRequest<Unit>;