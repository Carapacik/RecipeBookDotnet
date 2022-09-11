using RecipeBook.Application.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class UserAdapter
{
    public static UserCommand FromDto(this UserDto userDto)
    {
        return new UserCommand
        {
            Password = userDto.Password,
            Email = userDto.Email
        };
    }
}