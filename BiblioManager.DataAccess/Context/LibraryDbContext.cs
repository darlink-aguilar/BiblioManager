using Microsoft.EntityFrameworkCore;
using BiblioManager.Domain.Entities;

namespace BiblioManager.DataAccess.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        // Un DbSet<T> representa una tabla en la base de datos.
        public DbSet<Author> Authors => Set<Author>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Author Configuration ──
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.Id); //Esto significa que es la llave
                entity.Property(a => a.FirstName)
                      .IsRequired() // Este campo es obligatorio
                      .HasMaxLength(50); // Maximo 50 caracteres 
                entity.Property(a => a.LastName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.Nationality)
                      .IsRequired()
                      .HasMaxLength(60);
                entity.Property(a => a.BirthDate);
                entity.Property(a => a.CreatedAt)
                      .IsRequired();
                entity.Property(a => a.UpdatedAt)
                      .IsRequired(false); 
            });
        }
    }
}