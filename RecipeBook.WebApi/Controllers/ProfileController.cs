using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.Services;
using RecipeBook.WebApi.Adapters;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileDto>> Profile()
    {
        var profile = await _profileService.GetProfile();
        if (profile == null) return BadRequest("Profile not found.");
        return Ok(profile.ToDto());
    }

    [HttpPatch("profile/edit")]
    public async Task EditProfile(EditProfileDto editProfileDto)
    {
        await _profileService.EditProfile(editProfileDto.FromDto());
    }
}