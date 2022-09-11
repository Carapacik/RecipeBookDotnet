using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Configs;

public class RecipeConfig : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.HasKey(x => x.RecipeId);
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        builder.Property(x => x.ImageUrl).IsRequired();
        builder.Property(x => x.CookingTimeInMinutes).IsRequired();
        builder.Property(x => x.PortionsCount).IsRequired();
        builder.Property(x => x.CreationDateTime).IsRequired();
    }
}