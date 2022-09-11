namespace RecipeBook.WebApi.DTO;

public class DetailRecipeDto
{
    public int? RecipeId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int CookingTimeInMinutes { get; set; }
    public int PortionsCount { get; set; }
    public int LikesCount { get; set; }
    public int FavoritesCount { get; set; }
    public string? AuthorName { get; set; }
    public bool IsLiked { get; set; }
    public bool IsFavorite { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Steps { get; set; }
    public List<IngredientDto> Ingredients { get; set; }
}