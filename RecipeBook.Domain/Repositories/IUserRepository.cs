using RecipeBook.Domain.Entities;

namespace RecipeBook.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task UpdateUser(User existingUser, User editedUser);
    Task<User?> GetByEmail(string email);
    Task<User?> GetById(int id);
    Task<IReadOnlyList<User>> GetByIds(IEnumerable<int> ids);
}