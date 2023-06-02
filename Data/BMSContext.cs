using BookManagementSystem_BMS.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem_BMS.Data
{
    public class BMSContext : DbContext
    {
        public BMSContext() { }

        public BMSContext(DbContextOptions<BMSContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID);
            modelBuilder.Entity<User>()
                .HasAlternateKey(c => c.EmailAddress)
                .HasName("EmailAddress");

            modelBuilder.Entity<Role>()
            .HasMany(r => r.Categories)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("Roles_Categories"));

            modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryID);

            modelBuilder.Entity<Chapter>()
            .HasOne(c => c.Book)
            .WithMany(b => b.Chapters)
            .HasForeignKey(c => c.BookID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
