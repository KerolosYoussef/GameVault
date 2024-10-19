using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Categories.Data
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            ConfigureCategoryTable(builder);
        }

        private void ConfigureCategoryTable(EntityTypeBuilder<Category> builder)
        {
            // Table name
            builder.ToTable("Categories");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(200);

            builder.Property(c => c.Status)
                .IsRequired();

            // Relationships
            builder.HasOne(c => c.ParentCategory)
                .WithMany()  // Assuming ParentCategory does not have a navigation property back to Category
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the delete behavior

            builder.HasMany(c => c.Products)
            .WithMany(p => p.Categories)
            .UsingEntity<CategoryProduct>(
            j => j
                .HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId),
            j => j
                .HasOne(cp => cp.Category)
                .WithMany()
                .HasForeignKey(cp => cp.CategoryId),
            j =>
            {
                j.HasKey(cp => new { cp.ProductId, cp.CategoryId });
            });
        }
    }
}
