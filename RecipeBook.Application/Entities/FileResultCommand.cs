namespace RecipeBook.Application.Entities;

public class FileResultCommand
{
    public byte[] Content { get; set; }
    public string? Extension { get; set; }
}