namespace RecipeBook.WebApi.Adapters;

public class FormFileAdapter
{
    private FormFileAdapter(string fileExtension, byte[] data)
    {
        FileExtension = fileExtension;
        Data = data;
    }

    public byte[] Data { get; }
    public string FileExtension { get; }

    public static async Task<FormFileAdapter?> Create(IFormFile? formFile)
    {
        if (formFile == null) return null;

        byte[] bytes;
        await using (MemoryStream memoryStream = new())
        {
            await using (var fileStream = formFile.OpenReadStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }
        }

        return new FormFileAdapter(formFile.FileName.Split('.').Last(), bytes);
    }
}