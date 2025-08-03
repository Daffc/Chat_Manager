using Application.Commands.LoginUser;
using Application.DTOs.Responses;
using Domain.Exceptions;
using Domain.Interfaces;
using MediatR;
public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtServie;

    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtServie = jwtService;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(command.Email);

        if (user == null)
        {
            throw new NotFoundException("Invalid email or password.");
        }

        if (!_passwordHasher.Verify(command.Password, user.Password))
        {
            throw new ArgumentException("Invalid email or password.");
        }

        var token = _jwtServie.GenerateToken(user);
        return new AuthResponse(user.Id, user.NickName, user.Email, token);
    }
}