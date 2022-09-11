using RecipeBook.Application.Entities;

namespace RecipeBook.Application.Services;

public interface IAuthService
{
    Task<string> Login(UserCommand user);
    Task<string> Register(RegisterUserCommand user);
    Task ValidateUser();
}