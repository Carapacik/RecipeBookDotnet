namespace RecipeBook.Domain.Entities;

public class Rating
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public bool InFavorite { get; set; }
    public bool IsLiked { get; set; }
    public DateTime ModificationDateTime { get; set; }
}