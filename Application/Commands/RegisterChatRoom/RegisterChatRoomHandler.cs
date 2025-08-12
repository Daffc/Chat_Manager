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
    private readonly IUnitOfWork _unitOfWork;
    public RegisterChatRoom(
        IChatRoomRepository chatRoomRepository,
        IChatRoomMemberRepository chatRoomMemberRepository,
        IUserRepository userRepository,
        IValidator<RegisterChatRoomCommand> validator,
        IIdentityService identityService,
        IUnitOfWork unitOfWork
    )
    {
        _chatRoomRepository = chatRoomRepository;
        _chatRoomMemberRepository = chatRoomMemberRepository;
        _userRepository = userRepository;
        _validator = validator;
        _identityService = identityService;
        _unitOfWork = unitOfWork;
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
            ownerId: currentUserId,
            name: command.Name
        );
        await _chatRoomRepository.AddAsync(chatRoom);

        // Adding owner as a ChatRoomMember
        var ownerMember = new ChatRoomMember(
            userId: currentUserId,
            chatRoomId: chatRoom.Id
        );
        await _chatRoomMemberRepository.AddAsync(ownerMember);

        // Persisting new instances in the database.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChatRoomResponse(chatRoom.Id, chatRoom.Name);
    }
}