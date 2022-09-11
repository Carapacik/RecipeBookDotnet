namespace RecipeBook.Application.Entities;

public class DailyRecipeCommand
{
    public int RecipeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int LikesCount { get; set; }
    public int CookingTimeInMinutes { get; set; }
    public string? Author { get; set; }
}