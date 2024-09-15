using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Attributes.Data
{
    public class AttributeConfiguration : IEntityTypeConfiguration<Models.Attribute>
    {
        public void Configure(EntityTypeBuilder<Models.Attribute> builder)
        {
            ConfigureAttributeTable(builder);
        }

        private void ConfigureAttributeTable(EntityTypeBuilder<Models.Attribute> builder)
        {
            builder.ToTable("Attributes");

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
