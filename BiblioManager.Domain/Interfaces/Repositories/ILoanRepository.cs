using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Enums;

namespace BiblioManager.Domain.Interfaces.Repositories;

public interface ILoanRepository : IGenericRepository<Loan>
{
    Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId); // Obtiene los préstamos de un miembro específico
    Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status); // Obtiene todos los préstamos que están actualmente bajo un estado específico
    Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId); // Obtiene todos los prestamos de un miembro específico
    Task<IEnumerable<Loan>> GetExpiredLoansAsync(DateTime currentDate); // Busca prestamos cuya DueDate ya paso 
}