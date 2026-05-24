using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Repositories;

public interface IBookAuthorRepository : IGenericRepository<BookAuthor>
{
    Task<BookAuthor?> GetByBookAndAuthorAsync(int bookId, int authorId); // Verificar la existencia de una relación específica entre un libro y un autor
    Task<IEnumerable<BookAuthor>> GetByBookAsync(int bookId); // Obtener todas las relaciones de un libro específico
}