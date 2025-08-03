namespace Application.DTOs.Responses;

public record AuthResponse(Guid Id, string NickName, string Email, string Token);