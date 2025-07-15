using Application.Commands.RegisterUser;
using Domain.Interfaces;
using MediatR;

public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand command, CancellationToken cancelationToken){
        var user = new User(
            command.NickName,
            command.FirstName,
            command.LastName,
            command.Email,
            _passwordHasher.Hash(command.Password)
        );

        await _userRepository.AddAsync(user);
        return user.Id;
    }  
}