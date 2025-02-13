using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using web.api.Models;

namespace web.api.Infrastructure.Data
{
    public class HomeDbContext : IdentityDbContext<IdentityUser>
    {
        public HomeDbContext(DbContextOptions<HomeDbContext> options)
            : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>().ToTable("Todo");

            modelBuilder.Entity<Category>().ToTable("Categories");

            base.OnModelCreating(modelBuilder);
        }
    }
}
