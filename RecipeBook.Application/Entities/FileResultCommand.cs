namespace RecipeBook.Application.Entities;

public class FileResultCommand
{
    public FileResultCommand(byte[] content, string extension)
    {
        Content = content;
        Extension = extension;
    }

    public byte[] Content { get; init; }
    public string Extension { get; init; }
}