namespace RecipeBook.Application.Configs;

public class FileStorageSettings
{
    public FileStorageSettings(string basePath)
    {
        BasePath = basePath;
    }

    public string BasePath { get; }
}