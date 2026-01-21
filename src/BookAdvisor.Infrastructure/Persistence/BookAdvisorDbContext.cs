using BookAdvisor.Domain.Entities;
using BookAdvisor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookAdvisor.Infrastructure.Persistence
{
    public class BookAdvisorDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookAdvisorDbContext(DbContextOptions<BookAdvisorDbContext> options)
            : base(options){}

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
