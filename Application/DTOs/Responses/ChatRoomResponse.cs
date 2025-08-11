namespace Application.DTOs.Responses;

public sealed record ChatRoomResponse(
    Guid Id,
    string Name
);