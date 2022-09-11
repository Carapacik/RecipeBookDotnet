using RecipeBook.Application.Entities;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Adapters;

public static class EditProfileAdapter
{
    public static EditProfileCommand FromDto(this EditProfileDto editProfileDto)
    {
        return new EditProfileCommand
        {
            Password = editProfileDto.Password,
            Name = editProfileDto.Name,
            Description = editProfileDto.Description,
            Email = editProfileDto.Email
        };
    }
}