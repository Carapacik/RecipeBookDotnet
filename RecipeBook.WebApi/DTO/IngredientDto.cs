namespace RecipeBook.WebApi.DTO;

public class IngredientDto
{
    public string Title { get; set; } = string.Empty;
    public List<string> IngredientNames { get; set; } = new();
}