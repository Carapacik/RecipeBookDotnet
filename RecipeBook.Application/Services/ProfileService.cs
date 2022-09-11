using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Entities;
using RecipeBook.Application.Extensions;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure;

namespace RecipeBook.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRatingRepository _ratingRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public ProfileService(IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRatingRepository ratingRepository,
        IRecipeRepository recipeRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
        _recipeRepository = recipeRepository;
    }

    public async Task<ProfileCommand?> GetProfile()
    {
        var claimsEmail = ClaimsEmail();
        var user = await _userRepository.GetByEmail(claimsEmail);
        if (user == null) throw new Exception("User not found.");
        var favoritesCount = await _ratingRepository.GetUserFavoritesCountByUserId(user.UserId);
        var likesCount = await _ratingRepository.GetUserLikesCountByUserId(user.UserId);
        var recipesCount = await _recipeRepository.GetUserRecipesCountByUserId(user.UserId);
        return new ProfileCommand
        {
            RecipesCount = recipesCount,
            FavoritesCount = favoritesCount,
            LikesCount = likesCount,
            Name = user.Name,
            Description = user.Description
        };
    }

    public async Task EditProfile(EditProfileCommand editProfileCommand)
    {
        var claimsEmail = ClaimsEmail();
        var existingUser = await _userRepository.GetByEmail(claimsEmail);
        if (existingUser == null) throw new Exception($"User with email [{claimsEmail}] does not exist.");

        var editedUser = ConvertToUser(editProfileCommand);
        await _userRepository.UpdateUser(existingUser, editedUser);
        await _unitOfWork.Commit();
    }

    private static User ConvertToUser(EditProfileCommand editProfileCommand)
    {
        PasswordExtension.CreatePasswordHash(editProfileCommand.Password, out var passwordHash, out var passwordSalt);
        return new User
        {
            Description = editProfileCommand.Description,
            Name = editProfileCommand.Name,
            Email = editProfileCommand.Email,
            PasswordHash = passwordHash, PasswordSalt = passwordSalt
        };
    }

    private string ClaimsEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        if (email == null) throw new Exception("Invalid user.");
        return email;
    }
}