using Application.Commands.DeleteUser;
using Domain.Interfaces;
using Domain.Exceptions;
using MediatR;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;

    public DeleteUserHandler(IUserRepository userRepository, IIdentityService identityService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var currentUserId  = _identityService.GetCurrentUserId();

        if (currentUserId  != command.UserId)
        {
            throw new ForbiddenAccessException("You can only delete your own account.");
        }

        var deleted = await _userRepository.DeleteAsync(command.UserId);

        if (!deleted)
        {
            throw new NotFoundException($"User with ID '{command.UserId}' not found.");
        }

        return true;
    }
}