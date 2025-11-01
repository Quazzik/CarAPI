using CarAPI.Data;
using CarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAPI.Repositories;

public class AuthRepository
{
    private readonly AuthDbContext _context;
    private readonly DbSet<User> _dbSet;

    public AuthRepository(AuthDbContext context)
    {
        _context = context;
        _dbSet = context.Set<User>();
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        var res = await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
        return res;
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