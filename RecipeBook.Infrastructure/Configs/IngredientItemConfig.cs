using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Configs;

public class IngredientItemConfig : IEntityTypeConfiguration<IngredientItem>
{
    public void Configure(EntityTypeBuilder<IngredientItem> builder)
    {
        builder.HasKey(item => item.IngredientItemId);
        builder.Property(x => x.Name).IsRequired();
    }
}