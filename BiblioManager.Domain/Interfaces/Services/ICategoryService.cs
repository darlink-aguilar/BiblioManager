using BiblioManager.Domain.Entities;

namespace BiblioManager.Domain.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(Category category);
        Task UpdateAsync(int id, Category category);
        Task DeleteAsync(int id);
    }
}