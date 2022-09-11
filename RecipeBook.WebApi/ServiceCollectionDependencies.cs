using RecipeBook.Application.Services;
using RecipeBook.Domain.Repositories;
using RecipeBook.Infrastructure;
using RecipeBook.Infrastructure.Repositories;
using RecipeBook.WebApi.Builders;

namespace RecipeBook.WebApi;

public static class ServiceCollectionDependencies
{
    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddScoped<IRatingRepository, RatingRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IRatingService, RatingService>();
        services.AddScoped<IRecipeService, RecipeService>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<RecipeBuilder>();
    }
}