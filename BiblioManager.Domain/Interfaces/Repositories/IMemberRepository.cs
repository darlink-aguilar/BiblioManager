using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Repositories

{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member?> GetByDniAsync(string dni);// Obtener cedula
    }
}
