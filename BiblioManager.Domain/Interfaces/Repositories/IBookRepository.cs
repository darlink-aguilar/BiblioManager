using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Repositories

{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<Book?> GetByIsbnAsync(string isbn);// Obtener el isbn de un libro específico
    }
}
