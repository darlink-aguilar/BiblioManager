using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(int id);
        Task<Member> CreateAsync(Member member);
        Task UpdateAsync(int id, Member member);
        Task ActivateDeactivateAsync(int id); // No se elimina, solo se activa o desactiva
    }
}