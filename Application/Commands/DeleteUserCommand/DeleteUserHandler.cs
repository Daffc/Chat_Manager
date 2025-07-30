using Application.Commands.DeleteUser;
using Domain.Interfaces;
using MediatR;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAsync(command.userId);
        return Unit.Value;
    }
}