using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Requests;

public sealed record RegisterChatRoomRequest(
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Name must be between 5-50 character long")]
    string Name
);