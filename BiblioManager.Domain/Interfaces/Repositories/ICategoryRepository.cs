using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Repositories

{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetByNameAsync(string name);// Obtener nombre
        Task<bool> HasBooksAsync(int categoryId); // REGLA DE NEGOCIO: No se puede eliminar una categoría si tiene libros asociados
    }
}
