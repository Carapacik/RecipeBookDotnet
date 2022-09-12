using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RecipeBook.Infrastructure;

namespace RecipeBook.Migrations;

public class DbContextFactory : IDesignTimeDbContextFactory<RecipeBookDbContext>
{
    public RecipeBookDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<RecipeBookDbContext>();

        optionsBuilder.UseNpgsql(configuration.GetConnectionString("ConnectionString"), b =>
        {
            b.MigrationsAssembly("RecipeBook.Migrations");
            b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });

        return new RecipeBookDbContext(optionsBuilder.Options);
    }
}