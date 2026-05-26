using Microsoft.Extensions.Logging;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Enums;
using BiblioManager.Domain.Interfaces.Repositories;
using BiblioManager.Domain.Interfaces.Services;

namespace BiblioManager.Domain.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILogger<LoanService> _logger;

    public LoanService(
        ILoanRepository loanRepository,
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        ILogger<LoanService> logger)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all loans");
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving loan with ID: {LoanId}", id);
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan == null)
        {
            _logger.LogWarning("Loan with ID {LoanId} not found", id);
        }
        return loan;
    }

    public async Task<Loan> CreateAsync(Loan loan)
    {
        // VALIDACIONES
        // 1. Validar que el usuario existe y está activo
        var member = await _memberRepository.GetByIdAsync(loan.MemberId);
        if (member == null)
            throw new KeyNotFoundException($"No se encontró el usuario con ID {loan.MemberId}");

        if (!member.IsActive)  
            throw new InvalidOperationException("No se puede realizar un préstamo a un usuario inactivo");

        // 2. Validar que el libro existe
        var book = await _bookRepository.GetByIdAsync(loan.BookId);
        if (book == null)
            throw new KeyNotFoundException($"No se encontró el libro con ID {loan.BookId}");

        // 4. La fecha de entrega debe ser posterior a la fecha actual
        if (loan.DueDate <= DateTime.UtcNow)
            throw new InvalidOperationException("La fecha de devolución estimada debe ser posterior a la fecha actual");

        loan.LoanDate = DateTime.UtcNow;
        loan.Status = LoanStatus.Active;
        loan.ReturnDate = null;

        _logger.LogInformation("Creating loan for Member {MemberId} with Book {BookId}", loan.MemberId, loan.BookId);
        return await _loanRepository.CreateAsync(loan);
    }

    public async Task UpdateAsync(int id, Loan loan)
    {
        var existing = await _loanRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

        // Solo permitimos editar el préstamo sigue activo
        if (existing.Status != LoanStatus.Active)
            throw new InvalidOperationException("Solo se pueden modificar las fechas de préstamos con estado activo");

        if (loan.DueDate <= existing.LoanDate)
            throw new InvalidOperationException("La nueva fecha de devolución debe ser posterior a la fecha en que se inició el préstamo");

        existing.DueDate = loan.DueDate;

        _logger.LogInformation("Updating DueDate for loan with ID: {LoanId}", id);
        await _loanRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _loanRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

        // Solo se puede eliminar registros de préstamos que estén activos o devueltos
        if (existing.Status == LoanStatus.Overdue)
            throw new InvalidOperationException("No se pueden eliminar registros de préstamos que se encuentran vencidos");

        _logger.LogInformation("Deleting loan with ID: {LoanId}", id);
        await _loanRepository.DeleteAsync(id);
    }

    public async Task UpdateStatusAsync(int id, LoanStatus newStatus)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan == null)
            throw new KeyNotFoundException($"No se encontró el préstamo con ID {id}");

        // Validar transiciones de estado 
        var validTransition = (loan.Status, newStatus) switch
        {
            (LoanStatus.Active, LoanStatus.Returned) => true,
            (LoanStatus.Active, LoanStatus.Overdue) => true,
            (LoanStatus.Overdue, LoanStatus.Returned) => true,
            _ => false
        };

        if (!validTransition)
            throw new InvalidOperationException($"No se permite cambiar el estado del préstamo de {loan.Status} a {newStatus}");

        loan.Status = newStatus;

        _logger.LogInformation("Updating loan {LoanId} status to {NewStatus}", id, newStatus);
        await _loanRepository.UpdateAsync(loan);
    }
} 