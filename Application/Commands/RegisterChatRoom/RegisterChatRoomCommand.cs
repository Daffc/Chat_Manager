using Application.DTOs.Responses;
using MediatR;

namespace Application.Commands.RegisterChatRoom;

public sealed record RegisterChatRoomCommand(
    string Name
): IRequest<ChatRoomResponse>;
