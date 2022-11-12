using RecipeBook.Application.Configs;
using RecipeBook.Application.Entities;

namespace RecipeBook.Application.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageSettings _fileStorageSettings;

    public FileStorageService(FileStorageSettings fileStorageSettings)
    {
        _fileStorageSettings = fileStorageSettings;
    }

    public async Task<FileResultCommand> GetFile(string path)
    {
        return new FileResultCommand(
            await File.ReadAllBytesAsync($"{_fileStorageSettings.BasePath}\\{path}"),
            path.Split('.').Last());
    }

    public Task RemoveFile(string path, string fileName)
    {
        File.Delete($"{_fileStorageSettings.BasePath}\\{path}\\{fileName}");
        return Task.CompletedTask;
    }

    public async Task<SaveFileResultCommand?> SaveFile(FileResultCommand? file, string path)
    {
        if (file is null) return null;
        var fileName = $"{Guid.NewGuid().ToString()}.{file.Extension}";
        var newFilePath = $"{_fileStorageSettings.BasePath}\\{path}\\{fileName}";
        await File.WriteAllBytesAsync(newFilePath, file.Content);
        return new SaveFileResultCommand
        {
            RelativeUri = fileName
        };
    }
}