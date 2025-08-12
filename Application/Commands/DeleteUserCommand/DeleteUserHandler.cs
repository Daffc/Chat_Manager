using Application.Commands.DeleteUser;
using Domain.Interfaces;
using Domain.Exceptions;
using MediatR;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserHandler(
        IUserRepository userRepository,
        IIdentityService identityService,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var currentUserId = _identityService.GetCurrentUserId();

        if (currentUserId != command.UserId)
        {
            throw new ForbiddenAccessException("You can only delete your own account.");
        }

        var deleted = await _userRepository.DeleteAsync(command.UserId);

        if (!deleted)
        {
            throw new NotFoundException($"User with ID '{command.UserId}' not found.");
        }

        // Persisting soft-delete in database.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}