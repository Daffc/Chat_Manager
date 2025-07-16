namespace Domain.Interfaces;


public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
}