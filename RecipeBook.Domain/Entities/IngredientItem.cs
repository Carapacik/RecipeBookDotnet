namespace RecipeBook.Domain.Entities;

public class IngredientItem
{
    public int IngredientItemId { get; set; }
    public string Name { get; set; }

    public int IngredientId { get; set; }
}