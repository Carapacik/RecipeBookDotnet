namespace RecipeBook.Application.Entities;

public class ProfileCommand
{
    public int RecipesCount { get; set; }
    public int LikesCount { get; set; }
    public int FavoritesCount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
}