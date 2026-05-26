using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Enums;

namespace BiblioManager.Domain.Interfaces.Services;

public interface ILoanService
{
    Task<IEnumerable<Loan>> GetAllAsync();
    Task<Loan?> GetByIdAsync(int id);
    Task<Loan> CreateAsync(Loan loan);
    Task UpdateAsync(int id, Loan loan);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, LoanStatus newStatus); 
}