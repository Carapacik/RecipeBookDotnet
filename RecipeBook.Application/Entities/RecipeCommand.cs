using RecipeBook.Domain.Entities;

namespace RecipeBook.Application.Entities;

public class RecipeCommand
{
    public RecipeCommand(
        int id,
        string title,
        string description,
        int cookingTimeInMinutes,
        int portionsCount,
        string email,
        List<string> tags,
        List<string> steps,
        List<Ingredient> ingredients,
        FileResultCommand? storageFile)
    {
        RecipeId = id;
        Title = title;
        Description = description;
        CookingTimeInMinutes = cookingTimeInMinutes;
        PortionsCount = portionsCount;
        Email = email;
        Tags = tags;
        Steps = steps;
        Ingredients = ingredients;
        StorageFile = storageFile;
    }

    public int RecipeId { get; }
    public string Title { get; }
    public string Description { get; }
    public int CookingTimeInMinutes { get; }
    public int PortionsCount { get; }
    public string Email { get; }
    public List<string> Tags { get; }
    public List<string> Steps { get; }
    public List<Ingredient> Ingredients { get; }
    public FileResultCommand? StorageFile { get; }
}