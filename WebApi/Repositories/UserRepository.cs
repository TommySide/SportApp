using Microsoft.EntityFrameworkCore;
using SharedLibrary;
using SharedLibrary.Entities;

namespace WebApplication1.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersAsync() => await _context.Users.ToListAsync();
    
    public async Task<User> GetUserById(int id)
    {
        return await _context.Users
            .Where(u => u.Id == id)
            .Include(u => u.Activites)
            .Include(u => u.Reservations)
            .FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}