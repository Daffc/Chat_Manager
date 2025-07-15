using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Requests;
using Application.Commands.RegisterUser;
using Application.Queries.GetUser;
using Application.DTOs.Responses;
using MediatR;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.NickName,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var userId = await _mediator.Send(command);
        return Ok(userId);
    }

    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        var query = new GetUserQuery(userId);
        var user = await _mediator.Send(query);
        return Ok(user);
    }
}