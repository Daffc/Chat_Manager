using Application.Commands.RegisterChatRoom;
using Application.DTOs.Responses;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using FluentValidation;
using MediatR;

public sealed class RegisterChatRoom : IRequestHandler<RegisterChatRoomCommand, ChatRoomResponse>
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IChatRoomMemberRepository _chatRoomMemberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterChatRoomCommand> _validator;
    private readonly IIdentityService _identityService;
    public RegisterChatRoom(
        IChatRoomRepository chatRoomRepository,
        IChatRoomMemberRepository chatRoomMemberRepository,
        IUserRepository userRepository,
        IValidator<RegisterChatRoomCommand> validator,
        IIdentityService identityService
    )
    {
        _chatRoomRepository = chatRoomRepository;
        _chatRoomMemberRepository = chatRoomMemberRepository;
        _userRepository = userRepository;
        _validator = validator;
        _identityService = identityService;
    }

    public async Task<ChatRoomResponse> Handle(RegisterChatRoomCommand command, CancellationToken cancellationToken)
    {

        var currentUserId  = _identityService.GetCurrentUserId();

        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        if (await _userRepository.GetByIdAsync(currentUserId) == null)
        {
            throw new NotFoundException($"User with ID '{currentUserId}' not found.");

        }

        // Creating ChatRoom
        var chatRoom = new ChatRoom(
            currentUserId,
            command.Name
        );
        await _chatRoomRepository.AddAsync(chatRoom);

        // Adding owner as a ChatRoomMember
        var ownerMember = new ChatRoomMember(
            userId: currentUserId,
            chatRoomId: chatRoom.Id
        );
        await _chatRoomMemberRepository.AddAsync(ownerMember);
            
        return new ChatRoomResponse(chatRoom.Id, chatRoom.Name);
    }
}