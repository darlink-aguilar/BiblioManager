using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Enums;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories;

public class LoanRepository : GenericRepository<Loan>, ILoanRepository
{
    public LoanRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Loan>> GetByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .AsNoTracking() // Significa que no vamos a modificar estos registros, solo leerlos
            .Where(l => l.MemberId == memberId)
            .Include(l => l.Book) 
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(l => l.Status == status)
            .Include(l => l.Book)
            .Include(l => l.Member) 
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansByMemberIdAsync(int memberId)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(l => l.MemberId == memberId &&
                       (l.Status == LoanStatus.Active || l.Status == LoanStatus.Overdue)) // Subconsulta 
            .Include(l => l.Book)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetExpiredLoansAsync(DateTime currentDate)
    {
        return await _dbSet
            .Where(l => l.Status == LoanStatus.Active && l.DueDate < currentDate)
            .ToListAsync();
    }
}