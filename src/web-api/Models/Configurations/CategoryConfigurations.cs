using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace web.api.Models.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Description).HasDefaultValue(string.Empty);
            builder.Property(c => c.timeStamp).IsRequired();
        }
    }
}
