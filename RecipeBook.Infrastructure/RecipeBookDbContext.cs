using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;
using RecipeBook.Infrastructure.Configs;

namespace RecipeBook.Infrastructure;

public class RecipeBookDbContext : DbContext
{
    public RecipeBookDbContext(DbContextOptions<RecipeBookDbContext> options) : base(options)
    {
    }

    public DbSet<Ingredient>? Ingredients { get; set; }
    public DbSet<IngredientItem>? IngredientItems { get; set; }
    public DbSet<Rating>? Ratings { get; set; }
    public DbSet<Recipe>? Recipes { get; set; }
    public DbSet<Step>? Steps { get; set; }
    public DbSet<Tag>? Tags { get; set; }
    public DbSet<User>? Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IngredientConfig());
        modelBuilder.ApplyConfiguration(new IngredientItemConfig());
        modelBuilder.ApplyConfiguration(new RatingConfig());
        modelBuilder.ApplyConfiguration(new RecipeConfig());
        modelBuilder.ApplyConfiguration(new StepConfig());
        modelBuilder.ApplyConfiguration(new TagConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());
    }
}