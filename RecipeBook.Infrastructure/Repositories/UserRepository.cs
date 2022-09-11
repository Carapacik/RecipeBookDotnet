using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RecipeBookDbContext _context;

    public UserRepository(RecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task CreateUser(User user)
    {
        await _context.Users!.AddAsync(user);
    }

    public Task UpdateUser(User existingUser, User editedUser)
    {
        existingUser.Name = editedUser.Name;
        existingUser.Description = editedUser.Description;
        existingUser.Email = editedUser.Email;
        editedUser.PasswordHash = existingUser.PasswordHash;
        editedUser.PasswordSalt = existingUser.PasswordSalt;
        return Task.CompletedTask;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users!.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetById(int id)
    {
        return await _context.Users!.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<IReadOnlyList<User>> GetByIds(IEnumerable<int> ids)
    {
        return await _context.Users!.Where(x => ids.Contains(x.UserId)).ToListAsync();
    }
}