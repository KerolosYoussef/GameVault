using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Warehouses.Data
{
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            ConfigureWarehouseTable(builder);
        }

        private void ConfigureWarehouseTable(EntityTypeBuilder<Warehouse> builder)
        {
            // Table name
            builder.ToTable("Warehouses");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(500);

            builder.Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(200);


            // Relationships
            builder.HasMany(x => x.Products)
                .WithMany(t => t.Warehouses)
                .UsingEntity<ProductWarehouse>(
                    j => j
                        .HasOne(ut => ut.Product)
                        .WithMany()
                        .HasForeignKey(ut => ut.ProductId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne(ut => ut.Warehouse)
                        .WithMany()
                        .HasForeignKey(ut => ut.WarehouseId)
                        .OnDelete(DeleteBehavior.Cascade))
                .HasIndex(ut => new { ut.ProductId, ut.WarehouseId })
                .IsUnique();
        }
    }
}
