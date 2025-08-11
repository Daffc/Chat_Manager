using Application.Commands.RegisterChatRoom;
using FluentValidation;

public class RegisterChatRoomCommandValidator : AbstractValidator<RegisterChatRoomCommand>
{
    public RegisterChatRoomCommandValidator()
    {
        RuleFor(cr => cr.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(5).WithMessage("Name must be at least 5 character long")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");
    }
}