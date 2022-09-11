using RecipeBook.Application.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class ProfileAdapter
{
    public static ProfileDto ToDto(this ProfileCommand profileCommand)
    {
        return new ProfileDto
        {
            RecipesCount = profileCommand.RecipesCount,
            LikesCount = profileCommand.LikesCount,
            FavoritesCount = profileCommand.FavoritesCount,
            Name = profileCommand.Name,
            Description = profileCommand.Description,
            Login = profileCommand.Login
        };
    }
}