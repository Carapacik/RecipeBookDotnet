using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.Services;
using RecipeBook.WebApi.Adapters;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterUserDto request)
    {
        return await _authService.Register(request.FromDto());
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        return await _authService.Login(request.FromDto());
    }

    [HttpGet("validate")]
    [Authorize]
    public async Task<IActionResult> Validate()
    {
        await _authService.ValidateUser();
        return Ok();
    }
}