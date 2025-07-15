using Application.DTOs.Responses;
using MediatR;

namespace Application.Queries.GetUser;

public sealed record GetUserQuery(Guid UserId) : IRequest<UserResponse>;