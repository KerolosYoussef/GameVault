using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Brands.Data
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            ConfigureBrandTable(builder);
        }

        private void ConfigureBrandTable(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("Brands");

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Brand)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
