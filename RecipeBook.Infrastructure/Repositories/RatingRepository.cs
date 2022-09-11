using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;

namespace RecipeBook.Infrastructure.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly RecipeBookDbContext _context;

    public RatingRepository(RecipeBookDbContext context)
    {
        _context = context;
    }

    public async Task Add(Rating rating)
    {
        await _context.Ratings!.AddAsync(rating);
    }

    public async Task<Rating?> Get(int userId, int recipeId)
    {
        return await _context.Ratings!.FirstOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);
    }

    public async Task<IReadOnlyList<Rating>> Get(int userId, IEnumerable<int> recipeIds)
    {
        return await _context.Ratings!.Where(x => x.UserId == userId && recipeIds.Contains(x.RecipeId)).ToListAsync();
    }

    public async Task<IReadOnlyList<Rating>> GetInFavoriteByUserId(int userId)
    {
        return await _context.Ratings!
            .Where(x => x.UserId == userId && x.InFavorite)
            .OrderByDescending(x => x.ModificationDateTime)
            .ToListAsync();
    }

    public async Task<int> GetUserLikesCountByUserId(int userId)
    {
        return await _context.Ratings!.CountAsync(x => x.UserId == userId && x.IsLiked);
    }

    public async Task<int> GetUserFavoritesCountByUserId(int userId)
    {
        return await _context.Ratings!.CountAsync(x => x.UserId == userId && x.InFavorite);
    }
}