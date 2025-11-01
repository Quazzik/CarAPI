using CarAPI.Data;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Repositories;

public class UserRepository
{
    private readonly AuthDbContext _context;
    private readonly DbSet<User> _dbSet;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
        _dbSet = context.Set<User>();
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<User> AddAsync(User user)
    {
        await _dbSet.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string login)
    {
        return await _dbSet.AnyAsync(u => u.Login == login);
    }
}