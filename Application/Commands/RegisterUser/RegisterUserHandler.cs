using Application.Commands.RegisterUser;
using Application.DTOs.Responses;
using Domain.Interfaces;
using Domain.Entities;
using MediatR;
using FluentValidation;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<RegisterUserCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IValidator<RegisterUserCommand> validator,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }

        if (await _userRepository.ExistsByEmail(command.Email))
        {
            throw new ArgumentException("Email already exists", nameof(command.Email));
        }

        var user = new User(
            command.NickName,
            command.FirstName,
            command.LastName,
            command.Email,
            _passwordHasher.Hash(command.Password)
        );
        await _userRepository.AddAsync(user);

        // Persisting soft-delete in database.
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.NickName, user.FirstName, user.LastName, user.Email);
    }
}