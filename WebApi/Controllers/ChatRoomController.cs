
using Application.Commands.RegisterChatRoom;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/chatrooms")]
[SwaggerTag("ChatRoom management endpoints")]
public sealed class ChatRoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatRoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = " Register new ChatRoom",
        Description = " Name requirements:\n" +
                    "- 5-50 chatacters"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "ChatRoom created", typeof(ChatRoomResponse))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation or general errors", typeof(ValidationProblemDetails))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Server error", typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Register(
        [FromBody] RegisterChatRoomRequest request,
        [FromServices] IIdentityService identityService)
    {
        var command = new RegisterChatRoomCommand(
            Name: request.Name
        );

        var chatRoom = await _mediator.Send(command);
        return Ok(chatRoom);
    }
}