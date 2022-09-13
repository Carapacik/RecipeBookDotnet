using RecipeBook.Application.Entities;

namespace RecipeBook.Application.Services;

public interface IFileStorageService
{
    Task<FileResultCommand> GetFile(string path);
    Task RemoveFile(string path, string fileName);
    Task<SaveFileResultCommand?> SaveFile(FileResultCommand? file, string path);
}