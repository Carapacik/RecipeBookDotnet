using Microsoft.AspNetCore.Mvc;
using RecipeBook.Application.Services;

namespace RecipeBook.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class StorageController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public StorageController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpGet("images/{filePath}")]
    public async Task<IActionResult> GetImage(string filePath)
    {
        if (filePath.Contains('\\')) return BadRequest();

        var result = await _fileStorageService.GetFile($"images\\{filePath}");
        if (result.Extension == null) return BadRequest();

        return new FileContentResult(result.Content, $"image/{result.Extension}");
    }
}