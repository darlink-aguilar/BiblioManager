using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task UpdateAsync(int id, Book book);
    Task DeleteAsync(int id);

    // Métodos para la relación con BookAuthor
    Task AddAuthorToBookAsync(int bookId, int authorId); // Relaconar un autor con un libro
    Task RemoveAuthorFromBookAsync(int bookId, int authorId); // Eliminar el registro en BookAuthor
    Task<IEnumerable<Author>> GetAuthorsByBookAsync(int bookId); // Obtener los autores relacionados a un libro
}