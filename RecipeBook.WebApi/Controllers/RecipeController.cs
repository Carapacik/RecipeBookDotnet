using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecipeBook.Application.Entities;
using RecipeBook.Application.Services;
using RecipeBook.WebApi.Adapters;
using RecipeBook.WebApi.Builders;
using RecipeBook.WebApi.DTO;

namespace RecipeBook.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public class RecipeController : ControllerBase
{
    private readonly RecipeBuilder _recipeBuilder;
    private readonly IRecipeService _recipeService;

    public RecipeController(RecipeBuilder recipeBuilder, IRecipeService recipeService)
    {
        _recipeBuilder = recipeBuilder;
        _recipeService = recipeService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<List<RecipeDto>> GetAllRecipes(int skip, int take, string searchQuery)
    {
        var recipes = await _recipeService.GetAllRecipes(skip, take, searchQuery);
        return await _recipeBuilder.BuildRecipes(recipes);
    }

    [HttpGet("daily")]
    [AllowAnonymous]
    public async Task<ActionResult<DailyRecipeDto>> GetRecipeOfDay()
    {
        var recipe = await _recipeService.GetRecipeOfDay();
        if (recipe == null) return BadRequest("Recipe of day does not exist");
        return Ok(recipe.ToDto());
    }

    [HttpGet("favorites")]
    public async Task<ActionResult<List<RecipeDto>>> GetFavoriteRecipes(int skip, int take)
    {
        var searchResult = await _recipeService.GetFavoriteRecipes(skip, take);
        return await _recipeBuilder.BuildRecipes(searchResult);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> AddRecipe()
    {
        await _recipeService.AddRecipe(await ParseRecipeCommand(Request.Form));
        return Ok();
    }

    [HttpDelete("{id:int}/delete")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        await _recipeService.DeleteRecipe(id);
        return Ok();
    }

    [HttpPatch("{id:int}/edit")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> EditRecipe(int id, IFormCollection form)
    {
        await _recipeService.EditRecipe(await ParseRecipeCommand(Request.Form, id));
        return Ok();
    }

    [HttpGet("user-owned")]
    public async Task<ActionResult<List<RecipeDto>>> GetUserOwnedRecipes(int skip, int take)
    {
        var recipes = await _recipeService.GetUserOwnedRecipes(skip, take);
        return await _recipeBuilder.BuildRecipes(recipes);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<DetailRecipeDto>> GetDetailRecipe(int id)
    {
        var recipe = await _recipeService.GetDetailRecipe(id);
        if (recipe == null) return BadRequest($"Recipe with id [{id}] does not exist");
        return await _recipeBuilder.BuildRecipeDetail(recipe);
    }

    private async Task<RecipeCommand> ParseRecipeCommand(IFormCollection formCollection, int id = 0)
    {
        var recipeData = JsonConvert.DeserializeObject<CreateRecipeDto>(formCollection["data"]);
        if (recipeData == null) throw new ArgumentException("Data is null");

        recipeData.RecipeId = id;
        IFormFile? formFile = null;
        if (formCollection.Files.Count > 0) formFile = formCollection.Files[0];

        return recipeData.FromDto(await FormFileAdapter.Create(formFile),
            HttpContext.User.FindFirstValue(ClaimTypes.Email));
    }
}