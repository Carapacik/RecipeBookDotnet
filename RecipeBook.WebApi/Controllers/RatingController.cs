using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.Services;

namespace RecipeBook.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]/{recipeId:int}")]
[Produces("application/json")]
public class RatingController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    [HttpPost("favorite")]
    public async Task<IActionResult> AddToFavorites(int recipeId)
    {
        await _ratingService.AddToFavorites(recipeId);
        return Ok();
    }

    [HttpDelete("favorite")]
    public async Task<IActionResult> RemoveFromFavorites(int recipeId)
    {
        await _ratingService.RemoveFromFavorites(recipeId);
        return Ok();
    }

    [HttpPost("like")]
    public async Task<IActionResult> AddToLikes(int recipeId)
    {
        await _ratingService.AddToLikes(recipeId);
        return Ok();
    }

    [HttpDelete("like")]
    public async Task<IActionResult> RemoveFromLikes(int recipeId)
    {
        await _ratingService.RemoveFromLikes(recipeId);
        return Ok();
    }
}