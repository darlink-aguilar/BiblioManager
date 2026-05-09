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
        public DbSet<Member> Members => Set<Member>();

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

            // ── Member Configuration ──
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(m => m.Id); 
                entity.Property(m => m.Dni)
                      .IsRequired() 
                      .HasMaxLength(10); 
                entity.Property(m => m.FullName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(m => m.Email)
                      .IsRequired()
                      .HasMaxLength(60);
                entity.Property(m => m.BirthDate);
                entity.Property(m => m.IsActive)
                      .IsRequired();
                entity.Property(m => m.CreatedAt)
                      .IsRequired();
                entity.Property(m => m.UpdatedAt)
                      .IsRequired(false);
                entity.HasIndex(m => m.Dni) // Aseguramos que el DNI sea único en la base de datos
                      .IsUnique(); 
            });
        }
    }
}