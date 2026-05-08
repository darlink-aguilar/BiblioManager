using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Repositories

{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<Author?> GetByNameAndLastNameAsync(string firstName, string lastName);// Obtener nombre y apellido
    }
}
