using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Domain.Exceptions;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users
            .Where(u => u.DeletedAt == null)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _dbContext.Users
            .Where(u => u.Email == email && u.DeletedAt == null)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await _dbContext.Users
            .Where(u => u.DeletedAt == null)
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> DeleteAsync(Guid userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);

        if (user is null || user.DeletedAt != null)
        {
            throw new NotFoundException($"User with ID {userId} was not found.");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}