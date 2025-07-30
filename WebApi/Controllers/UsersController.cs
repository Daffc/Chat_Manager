using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Requests;
using Application.Commands.RegisterUser;
using Application.Queries.GetUser;
using Application.DTOs.Responses;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Produces("application/json")]
[SwaggerTag("User management endpoints")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Register new user",
        Description = "Password requirements:\n" +
                    "- 8-20 characters\n" +
                    "- 1 uppercase letter\n" +
                    "- 1 lowercase letter\n" +
                    "- 1 number\n" +
                    "- 1 special character")]
    [SwaggerResponse(StatusCodes.Status200OK, "User created", typeof(UserResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation or general errors", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Server error", typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.NickName,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password
        );

        var user = await _mediator.Send(command);
        return Ok(user);
    }

    [HttpGet("{userId:guid}")]
    [SwaggerOperation(
        Summary = "Get user details",
        Description = "Retrieves user information by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "User found", typeof(UserResponse))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Not found", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Server error", typeof(ValidationProblemDetails))]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser(
        [SwaggerParameter("User ID", Required = true)]
        Guid userId)
    {
        var query = new GetUserQuery(userId);
        var user = await _mediator.Send(query);
        return Ok(user);
    }
}