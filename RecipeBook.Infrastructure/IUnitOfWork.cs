namespace RecipeBook.Infrastructure;

public interface IUnitOfWork
{
    Task Commit();
}