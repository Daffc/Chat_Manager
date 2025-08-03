using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Commands.DeleteUser;
using Domain.Interfaces;
using Domain.Exceptions;
using MediatR;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var userIdFromToken = command.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? command.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userIdFromToken is null || !Guid.TryParse(userIdFromToken, out var parsedTokenUserId))
        {
            throw new ForbiddenAccessException("Invalid user identity.");
        }

        if (parsedTokenUserId != command.UserId)
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