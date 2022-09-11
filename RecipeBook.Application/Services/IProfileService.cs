using RecipeBook.Application.Entities;

namespace RecipeBook.Application.Services;

public interface IProfileService
{
    Task<ProfileCommand?> GetProfile();
    Task EditProfile(EditProfileCommand editProfileCommand);
}