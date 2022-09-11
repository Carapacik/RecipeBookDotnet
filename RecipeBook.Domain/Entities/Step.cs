namespace RecipeBook.Domain.Entities;

public class Step
{
    public int StepId { get; set; }
    public int Number { get; set; }
    public string Description { get; set; }
    public int RecipeId { get; set; }
}