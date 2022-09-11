using RecipeBook.Application.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class RegisterUserAdapter
{
    public static RegisterUserCommand FromDto(this RegisterUserDto registerUserDto)
    {
        return new RegisterUserCommand
        {
            Password = registerUserDto.Password,
            Email = registerUserDto.Email,
            Name = registerUserDto.Name
        };
    }
}