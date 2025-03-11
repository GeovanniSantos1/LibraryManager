using LibraryManager.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Persistence
{
    public class LibraryManagerDbContext : DbContext
    {
        public LibraryManagerDbContext(DbContextOptions<LibraryManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configuração do Book
            builder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ISBN).IsRequired().HasMaxLength(13);
                entity.Property(e => e.PublicationYear).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();
            });

            // Configuração do User
            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configuração do Loan
            builder.Entity<Loan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LoanDate).IsRequired();
                entity.Property(e => e.ReturnDate).IsRequired();

                // Relacionamento com User
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relacionamento com Book
                entity.HasOne<Book>()
                    .WithMany()
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }
    }
}
