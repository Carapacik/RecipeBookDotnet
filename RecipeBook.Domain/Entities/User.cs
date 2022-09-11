namespace RecipeBook.Domain.Entities;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime CreationDate { get; set; }
}