using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Products.Data
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            ConfigureProductTable(builder);
        }

        private void ConfigureProductTable(EntityTypeBuilder<Product> builder)
        {
            // Table name
            builder.ToTable("Products");

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

            builder.HasMany(p => p.Attributes)
                .WithOne(a => a.Product)  // Assuming ParentCategory does not have a navigation property back to Category
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the delete behavior
        }
    }
}
