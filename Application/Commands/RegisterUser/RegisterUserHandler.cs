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

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponse> Handle(RegisterUserCommand command, CancellationToken cancelationToken){
        var user = new User(
            command.NickName,
            command.FirstName,
            command.LastName,
            command.Email,
            _passwordHasher.Hash(command.Password)
        );

        var validator = new UserValidator();
        var validationResult = await validator.ValidateAsync(user);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.ToList());
        }

        if (await _userRepository.ExistsByEmail(command.Email))
        {
            throw new ArgumentException("Email already exists");
        }

        await _userRepository.AddAsync(user);

        return new UserResponse(
            user.Id,
            user.NickName,
            user.FirstName,
            user.LastName,
            user.Email
        );
    }  
}