using Application.DTOs.Responses;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.GetUser;

public sealed class GetUserHandler : IRequestHandler<GetUserQuery, UserResponse>
{
    private readonly IUserRepository _userReponsitory;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userReponsitory = userRepository;
    }

    public async Task<UserResponse> Handle(
        GetUserQuery query,
        CancellationToken cancellationToken
    )
    {
        var user = await _userReponsitory.GetByIdAsync(query.UserId)
            ?? throw new KeyNotFoundException($"User with ID {query.UserId} not found");

        return new UserResponse(
            user.Id,
            user.NickName,
            user.FirstName,
            user.LastName,
            user.Email
        );
    }
}