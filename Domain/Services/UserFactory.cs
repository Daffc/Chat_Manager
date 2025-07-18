using Domain.Entities;

public sealed class UserFactory
{
    private readonly IPasswordHasher _passwordHasher;

    public UserFactory(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User Create(string nickName, string firstName, string lastName, string email, string password)
    {
        return new User(nickName, firstName, lastName, email, _passwordHasher.Hash(password));
    }
}