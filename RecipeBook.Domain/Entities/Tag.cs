namespace RecipeBook.Domain.Entities;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; }
    public int RecipeId { get; set; }
}