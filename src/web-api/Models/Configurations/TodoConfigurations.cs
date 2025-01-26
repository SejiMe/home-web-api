namespace web.api.Models.Configurations
{
    public class TodoConfigurations : IEntityTypeConfiguration<Todo>
    {
        public void Configure(
            Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Todo> builder
        )
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.IsComplete).IsRequired();
            builder.Property(t => t.categoryId).IsRequired();
            builder.HasOne(t => t.category).WithMany(c => c.Todos).HasForeignKey(t => t.categoryId);
        }
    }
}
