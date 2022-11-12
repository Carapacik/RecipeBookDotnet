namespace RecipeBook.WebApi.DTO;

public class CreateRecipeDto
{
    public int RecipeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CookingTimeInMinutes { get; set; }
    public int PortionsCount { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Steps { get; set; } = new();
    public IFormFile RecipeImage { get; set; }
    public List<IngredientDto> Ingredients { get; set; } = new();
}