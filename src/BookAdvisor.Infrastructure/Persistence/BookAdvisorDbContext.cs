using BookAdvisor.Domain.Constants;
using BookAdvisor.Domain.Entities;
using BookAdvisor.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookAdvisor.Infrastructure.Persistence
{
    public class BookAdvisorDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookAdvisorDbContext(DbContextOptions<BookAdvisorDbContext> options)
            : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<ReadingList> ReadingLists { get; set; }
        public DbSet<ReadingListItem> ReadingListItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Book configuration
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(DomainConstants.Books.TitleMaxLength);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(DomainConstants.Books.AuthorMaxLength);
            });

            //Reading Configuration
            modelBuilder.Entity<ReadingList>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Name).IsRequired().HasMaxLength(DomainConstants.ReadingLists.NameMaxLength);
                    entity.HasMany(e => e.Items).WithOne().HasForeignKey(i => i.ReadingListId).OnDelete(DeleteBehavior.Cascade);
                }
            );

            //Review Configuration
            modelBuilder.Entity<Review>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Rating).IsRequired();    //Puan zorunlu
                    entity.Property(e => e.Comment).HasMaxLength(1000); //Yorum opsiyonel
                    entity.HasOne(r => r.Book).WithMany()   //bir kitabın birden fazla yorumu olur.
                    .HasForeignKey(r => r.BookId).OnDelete(DeleteBehavior.Cascade);    //Kitap silinirse yorumlar da silinir.
                    entity.HasIndex(r => new { r.BookId, r.UserId }).IsUnique();    //Bir kişi bir kere yorum yapabilir.

                }
            );

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
