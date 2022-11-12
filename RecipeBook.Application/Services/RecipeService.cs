using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RecipeBook.Application.Entities;
using RecipeBook.Domain.Entities;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure;

namespace RecipeBook.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRatingRepository _ratingRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;

    public RecipeService(IFileStorageService fileStorageService,
        IRecipeRepository recipeRepository,
        IUserRepository userRepository,
        IRatingRepository ratingRepository,
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _fileStorageService = fileStorageService;
        _recipeRepository = recipeRepository;
        _userRepository = userRepository;
        _ratingRepository = ratingRepository;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }


    public async Task<DailyRecipeCommand?> GetRecipeOfDay()
    {
        var recipe = await _recipeRepository.GetRecipeOfDay();
        if (recipe == null) throw new Exception("Daily recipe does not exist.");
        var author = await _userRepository.GetById(recipe.UserId);
        return new DailyRecipeCommand
        {
            RecipeId = recipe.RecipeId,
            Title = recipe.Title,
            Description = recipe.Description,
            ImageUrl = recipe.ImageUrl,
            CookingTimeInMinutes = recipe.CookingTimeInMinutes,
            LikesCount = recipe.LikesCount,
            Author = author?.Name
        };
    }

    public async Task<int> AddRecipe(RecipeCommand command)
    {
        var filePath = await _fileStorageService.SaveFile(command.StorageFile, "images");
        if (filePath == null)
            throw new Exception("File in request not found.");
        var user = await _userRepository.GetByEmail(command.Email);
        if (user == null)
            throw new Exception("User not found.");
        var recipe = ConvertToRecipe(command, filePath, user.UserId);
        await _recipeRepository.Add(recipe);
        await _unitOfWork.Commit();
        return recipe.RecipeId;
    }

    public async Task DeleteRecipe(int id)
    {
        var recipe = await _recipeRepository.GetById(id);
        if (recipe == null) throw new ValidationException($"Recipe with id:{id} does not exist");

        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null)
            throw new Exception("User not found.");
        if (user.UserId != recipe.UserId)
            throw new Exception("Incorrect user");

        await _fileStorageService.RemoveFile("images", recipe.ImageUrl);
        await _recipeRepository.Delete(id);
        await _unitOfWork.Commit();
    }

    public async Task EditRecipe(RecipeCommand editCommand)
    {
        var existingRecipe = await _recipeRepository.GetById(editCommand.RecipeId);
        if (existingRecipe == null)
            throw new Exception($"Recipe with id [{editCommand.RecipeId}] does not exist");

        var user = await _userRepository.GetByEmail(editCommand.Email);
        if (user == null)
            throw new Exception("User not found.");
        if (user.UserId != existingRecipe.UserId) throw new Exception("Incorrect user");

        SaveFileResultCommand? filePath = null;
        if (editCommand.StorageFile != null)
            filePath = await _fileStorageService.SaveFile(editCommand.StorageFile, "images");

        if (filePath != null)
            await _fileStorageService.RemoveFile("images", existingRecipe.ImageUrl);

        var editedRecipe = ConvertToRecipe(editCommand, filePath!, user.UserId);

        await _recipeRepository.Edit(existingRecipe, editedRecipe);
        await _unitOfWork.Commit();
    }

    public async Task<IReadOnlyList<Recipe>> GetAllRecipes(int skip, int take, string? searchQuery)
    {
        return await _recipeRepository.Search(skip, take, searchQuery);
    }

    public async Task<Recipe?> GetDetailRecipe(int id)
    {
        return await _recipeRepository.GetById(id);
    }

    public async Task<IReadOnlyList<Recipe>> GetFavoriteRecipes(int skip, int take)
    {
        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null)
            throw new Exception("User not found.");
        var ratings = await _ratingRepository.GetInFavoriteByUserId(user.UserId);
        var recipeIds = ratings.Select(x => x.RecipeId).ToList();
        return await _recipeRepository.Search(skip, take, recipeIds);
    }

    public async Task<IReadOnlyList<Recipe>> GetUserOwnedRecipes(int skip, int take)
    {
        var user = await _userRepository.GetByEmail(ClaimsEmail());
        if (user == null)
            throw new Exception("User not found.");
        var recipes = await _recipeRepository.GetInUserOwnedByUserId(user.UserId);
        var recipeIds = recipes.Select(x => x.RecipeId).ToList();
        return await _recipeRepository.Search(skip, take, recipeIds);
    }

    private static Recipe ConvertToRecipe(RecipeCommand recipeCommand, SaveFileResultCommand saveFileResult, int userId)
    {
        return new Recipe
        {
            ImageUrl = saveFileResult.RelativeUri,
            RecipeId = recipeCommand.RecipeId,
            Title = recipeCommand.Title,
            Description = recipeCommand.Description,
            CookingTimeInMinutes = recipeCommand.CookingTimeInMinutes,
            PortionsCount = recipeCommand.PortionsCount,
            UserId = userId,
            CreationDateTime = DateTime.Now,
            Tags = recipeCommand.Tags.Select(x => new Tag {Name = x}).ToList(),
            Steps = recipeCommand.Steps.Select((x, i) => new Step {Number = i + 1, Description = x}).ToList(),
            Ingredients = recipeCommand.Ingredients
        };
    }

    private string ClaimsEmail()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        if (email == null) throw new Exception("Invalid user.");
        return email;
    }
}