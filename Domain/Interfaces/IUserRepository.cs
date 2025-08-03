using Domain.Entities;

namespace Domain.Interfaces;


public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task<bool> ExistsByEmail(string email);
    Task DeleteAsync(Guid userId);
    Task<User?> GetByEmail(string email);
}