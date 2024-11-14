using Microsoft.EntityFrameworkCore;
using ab_project.Models;

namespace ab_project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Training> Trainings { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Training>().ToTable("Training");
            modelBuilder.Entity<Category>().ToTable("Category");
        }
    }
}
