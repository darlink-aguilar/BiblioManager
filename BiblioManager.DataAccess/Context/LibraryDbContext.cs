using BiblioManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

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
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();

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

                // Indice unico
                entity.HasIndex(m => m.Dni) // Aseguramos que el DNI sea único en la base de datos
                      .IsUnique(); 
            });

            // ── Category Configuration ──
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(c => c.Description)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(c => c.CreatedAt)
                      .IsRequired();
                entity.Property(c => c.UpdatedAt)
                      .IsRequired(false);

                //Indice Unico
                entity.HasIndex(c => c.Name) // Aseguramos que el nombre sea único en la base de datos
                      .IsUnique();
            });

            // ── Book Configuration ──
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Isbn)
                      .IsRequired()
                      .HasMaxLength(17);
                entity.Property(b => b.Title)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(b => b.Synopsis)
                      .IsRequired()
                      .HasMaxLength(900);
                entity.Property(b => b.PublicationDate);
                entity.Property(b => b.CreatedAt)
                      .IsRequired();
                entity.Property(b => b.UpdatedAt)
                      .IsRequired(false);

                // Relación 1:N con Category
                entity.HasOne(b => b.Category) // Un libro tiene una categoría
                      .WithMany(c => c.Books) // Una categoría tiene muchos libros
                      .HasForeignKey(b => b.CategoryId) // La clave foránea en la tabla de Libros que apunta a categoria
                      .OnDelete(DeleteBehavior.Restrict); // No se puede eliminar una categoría si tiene libros asociados

                // Indice único
                entity.HasIndex(b => b.Isbn)
                      .IsUnique();
            });

            // ── BookAuthor Configuration ──
            modelBuilder.Entity<BookAuthor>(entity =>
            {
                entity.HasKey(ba => ba.Id);
                entity.Property(ba => ba.CreatedAt)
                      .IsRequired();
                entity.Property(ba => ba.UpdatedAt)
                      .IsRequired(false);

                // Relación con Book
                entity.HasOne(ba => ba.Book) // Un registro de BookAuthor tiene un libro
                      .WithMany(b => b.BookAuthors) // Un libro tiene muchos registros de BookAuthor
                      .HasForeignKey(ba => ba.BookId) // La clave foránea en la tabla de BookAuthor que apunta al libro
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un libro, se borran sus registros de BookAuthor

                // Relación con Author
                entity.HasOne(ba => ba.Author) // Un registro de BookAuthor tiene un autor
                      .WithMany(a => a.BookAuthors) // Un autor tiene muchos registros de BookAuthor
                      .HasForeignKey(ba => ba.AuthorId) // La clave foránea en la tabla de BookAuthor que apunta al autor
                      .OnDelete(DeleteBehavior.Cascade); // Si se borra un autor, se borran sus registros de BookAuthor
                     
                
                // Índice único compuesto: un autor solo una vez por libro
                entity.HasIndex(ba => new { ba.BookId, ba.AuthorId })
                      .IsUnique();
            });
        }
    }
}