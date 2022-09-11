namespace RecipeBook.Domain.Entities;

public class Recipe
{
    public int RecipeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public int CookingTimeInMinutes { get; set; }
    public int PortionsCount { get; set; }
    public int LikesCount { get; set; }
    public int FavoritesCount { get; set; }
    public DateTime CreationDateTime { get; set; }
    public int UserId { get; set; }
    public List<Tag> Tags { get; set; }
    public List<Step> Steps { get; set; }
    public List<Ingredient> Ingredients { get; set; }
}